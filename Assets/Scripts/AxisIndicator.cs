using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisIndicator : MonoBehaviour
{
    public enum Axis { XY, YZ, XZ };
    public Axis axis;
    private readonly int segments = 50;
    LineRenderer line;

    private const float time = 0.5f;
    private float currentTimer;
    public bool draw;


    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.widthMultiplier = 0.05f;

        
        CreatePoints();
        line.enabled = false;
    }

    private void Update()
    {
        if (draw)
        {
            Color color = line.material.color;
            color.a = 1f;
            line.material.color = color;
            line.enabled = true;
            currentTimer = time;
            draw = false; //draw
        }

        if (line.enabled)
        {
            if (currentTimer > 0)
                currentTimer -= Time.deltaTime;
            else
            {
                //draw = false;
                currentTimer = 0f;
                FadeOut();
            }
        }
    }

    private void FadeOut()
    {
        if (line.material.color.a >= 0)
        {
            Color color = line.material.color;
            color.a -= Time.deltaTime;
            line.material.color = color;
        }
        else
            line.enabled = false;
    }

    public void CreatePoints()
    {
        float x = 0;
        float y = 0;
        float z = 0;

        float change = (float)(2 * Math.PI / segments);
        float angle = change;

        for (int i = 0; i < (segments + 1); i++)
        {
            if (axis == Axis.XY)
            {
                x = Mathf.Sin(angle) * 1.5f;
                y = Mathf.Cos(angle) * 1.5f;
                z = 0;
            }

            if (axis == Axis.YZ)
            {
                x = 0;
                y = Mathf.Sin(angle) * 1.5f;
                z = Mathf.Cos(angle) * 1.5f;
            }

            if (axis == Axis.XZ)
            {
                x = Mathf.Sin(angle) * 1.5f;
                y = 0;
                z = Mathf.Cos(angle) * 1.5f;
            }

            line.SetPosition(i, new Vector3(x, y, z));

            angle += change;
        }

    }

    public void SetDrawingAxis()
    {
        draw = true;
    }
}



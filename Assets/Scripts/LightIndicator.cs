using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicator : MonoBehaviour
{
    public Transform lightRotation;

    void Update()
    {
        transform.rotation = lightRotation.rotation;
    }
}

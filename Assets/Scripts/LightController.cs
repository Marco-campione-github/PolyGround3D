using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    private bool gyroEnabled;
    private Gyroscope gyro;

    private float xRotation;
    private float yRotation;

    private bool gyroMovementEnabled;
    //private float xGyroLastRotation;
    //private float yGyroLastRotation;
    //private float xGyroRotation;
    //private float yGyroRotation;

    //private Vector3 startEulerAngles;
    //private Vector3 startGyroAttitudeToEuler;

    private void Start()
    {
        gyroEnabled = EnableGyro(); //Check if device has gyroscope
        gyroMovementEnabled = true;
        transform.localRotation = Quaternion.identity; //Rotate light forward
    }

    private void Update()
    {
        if (gyroEnabled && gyroMovementEnabled)
        {
            xRotation += -gyro.rotationRateUnbiased.x;
            yRotation += -gyro.rotationRateUnbiased.y;
            transform.eulerAngles = new Vector3(xRotation * 2, yRotation * 2, 0);
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    public void ResetRotation()
    {
        xRotation = 0;
        yRotation = 0;
        transform.localRotation = Quaternion.identity; //Rotate light forward
    }

    public void ChangeGyroscopeMovement()
    {
        ResetRotation();
        if (gyroMovementEnabled)
            gyroMovementEnabled = false;
        else
            gyroMovementEnabled = true;
    }

}

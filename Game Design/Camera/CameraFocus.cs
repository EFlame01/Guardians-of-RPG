using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraFocus is a class that determines
/// if camera should be reset to it's 
/// original position.
/// </summary>
public class CameraFocus : MonoBehaviour
{
    //public static variable
    public static bool ResetCamera;

    //private variables
    private float start = 0f;
    private float end = 0.5f;

    public void FixedUpdate()
    {
        if(ResetCamera)
            ResetCameraFocus();
    }

    /// <summary>
    /// Sets the camera back from it's current
    /// location to the reset position (Vector3(0,0,0)).
    /// </summary>
    private void ResetCameraFocus()
    {
        float t = (float)(start/end);
        Vector3 a = transform.localPosition;
        Vector3 b = new Vector3(0, 0, 0);

        if(start >= end || transform.localPosition == b)
        {
            ResetCamera = false;
            transform.localPosition = b;
            start = 0f;
        }

        transform.localPosition = Vector3.Lerp(a, b, t);
        start += Time.fixedDeltaTime;
    }
}
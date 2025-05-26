using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public static bool ResetCamera;
    private float start = 0f;
    private float end = 0.5f;

    public void FixedUpdate()
    {
        if(ResetCamera)
        {
            // ResetCamera = false;
            ResetCameraFocus();
        }
    }

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
        Debug.Log(transform.localPosition);
    }
}
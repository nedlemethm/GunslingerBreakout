using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureScreen : MonoBehaviour
{
    [SerializeField] private Camera captureCamera;

    public void CaptureView(Vector3 location)
    {
        captureCamera.transform.position = location;
        captureCamera.Render();
    }
}

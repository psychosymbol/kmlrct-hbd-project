using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;
    [SerializeField]
    private float minCamX, maxCamX, minCamY, maxCamY;

    public bool zoomable = false;
    public bool moveable = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(zoomable) Zoom(Input.mouseScrollDelta.y);
        if(moveable) PanCamera();   
    }

    void PanCamera()
    {

        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 dif = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += dif;
            
            Vector3 newCam = cam.transform.position;
            newCam.x = Mathf.Clamp(newCam.x, minCamX, maxCamX);
            newCam.y = Mathf.Clamp(newCam.y, minCamY, maxCamY);
            cam.transform.position = newCam;
        }
    }

    void Zoom(float index)
    {
        float newSize = cam.orthographicSize + (zoomStep * (-index));

        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }
}

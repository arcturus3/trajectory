using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera camera;

    private bool panning = false;
    private Vector3 panPosition = new Vector3(0, 0, 0);
    private Vector3 lastMousePosition;

    void Awake() {
        camera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        if (Input.GetMouseButton(0)) {
            if (!panning) {
                panning = true;
            }
            else {
                panPosition += mouseDelta;
            }
        }
        else {
            panning = false;
        }
        lastMousePosition = Input.mousePosition;
        PanCamera();
    }

    void PanCamera() {
        var newPosition = camera.transform.position;
        newPosition.x = -0.05f * panPosition.x;
        newPosition.z = -0.05f * panPosition.y;
        camera.transform.position = newPosition;
    }
}

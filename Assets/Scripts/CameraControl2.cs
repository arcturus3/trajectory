using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl2 : MonoBehaviour
{
    private Camera camera;
    public float panSpeed;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = Mouse.current.delta.value;
        bool panning = Mouse.current.leftButton.isPressed;
        if (panning) {
            Vector3 offset = -panSpeed * new Vector3(delta.x, 0, delta.y);
            camera.transform.position += offset;
        }
    }
}

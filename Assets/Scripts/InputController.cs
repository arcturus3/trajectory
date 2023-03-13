using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputAction pointerPosition;
    public InputAction pointerMove;
    public InputAction panButton;
    public InputAction orbitButton;
    public InputAction waypointSetButton;
    public GameObject waypointMarker;
    private GameObject camera;
    private GameObject cameraAnchor;
    private InputMode inputMode;

    private GameObject waypointPreview;
    private List<GameObject> waypoints;

    private void OnEnable() {
        pointerMove.Enable();
        panButton.Enable();
        orbitButton.Enable();
        waypointSetButton.Enable();
    }

    private void OnDisable() {
        pointerMove.Disable();
        panButton.Disable();
        orbitButton.Disable();
        waypointSetButton.Disable();
    }

    private void Start() {
        camera = Camera.main.gameObject;
        cameraAnchor = camera.transform.parent.gameObject;
        waypointPreview = Instantiate(waypointMarker);
    }

    private void Update() {
        SetInputMode();
        switch (inputMode) {
            case InputMode.Navigate:
                HandlePan();
                HandleOrbit();
                HandleZoom();
                break;
            case InputMode.Edit:
                HandleWaypointSet();
                break;
        }
    }

    private void SetInputMode() {
        if (Keyboard.current.nKey.wasPressedThisFrame) {
            inputMode = InputMode.Navigate;
            waypointPreview.SetActive(false);
        }
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            inputMode = InputMode.Edit;
            waypointPreview.SetActive(true);
        }
    }

    private void HandlePan() {
        // TODO: smoothing mouse input
        // TODO: raycast to scene to determine drag delta
        if (panButton.IsPressed()) {
            Plane groundPlane = new Plane(Vector3.up, 0);
            float panSpeed = 0.025f;
            Vector2 moveDelta = pointerMove.ReadValue<Vector2>();
            Vector3 panDelta = moveDelta.x * GetPanRight() + moveDelta.y * GetPanForward();
            cameraAnchor.transform.Translate(-panSpeed * panDelta, Space.World);
        }
    }

    private void HandleOrbit() {
        // TODO: x axis rotation limits
        if (orbitButton.IsPressed()) {
            Vector2 moveDelta = pointerMove.ReadValue<Vector2>();
            float orbitSpeed = 0.1f;
            Vector3 orbitDelta = orbitSpeed * new Vector3(-moveDelta.y, moveDelta.x, 0);
            cameraAnchor.transform.eulerAngles += orbitDelta;
            Mathf.Clamp(cameraAnchor.transform.eulerAngles.x, 0, 90);
        }
    }

    private void HandleZoom() {

    }

    private void HandleWaypointSet() {
        Vector2 position = pointerPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        bool success = Physics.Raycast(ray, out hit);
        if (success) {
            waypointPreview.SetActive(true);
            waypointPreview.transform.position = hit.point;
            if (waypointSetButton.WasPressedThisFrame()) {
                GameObject waypoint = Instantiate(waypointMarker);
                waypoint.transform.position = hit.point;
                waypoints.Add(waypoint);
            }
        }
        else {
            waypointPreview.SetActive(false);
        }
    }

    private Vector3 GetPanForward() {
        float angle = cameraAnchor.transform.rotation.eulerAngles.y;
        return Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }

    private Vector3 GetPanRight() {
        float angle = cameraAnchor.transform.rotation.eulerAngles.y;
        return Quaternion.Euler(0, angle, 0) * Vector3.right;
    }

    private enum InputMode {
        Navigate,
        Edit
    }
}

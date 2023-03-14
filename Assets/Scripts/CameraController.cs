using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool panning;
    private bool orbiting;

    private Transform cameraTransform;
    private Transform cameraAnchorTransform;

    private void Start() {
        panning = false;
        orbiting = false;
        cameraTransform = Camera.main.transform;
        cameraAnchorTransform = cameraTransform.parent;
    }

    private void OnEnable() {
        InputController.onPointerMove += HandlePointerMove;
        InputController.onPanStart += HandlePanStart;
        InputController.onPanEnd += HandlePanEnd;
        InputController.onOrbitStart += HandleOrbitStart;
        InputController.onOrbitEnd += HandleOrbitEnd;
    }

    private void OnDisable() {
        InputController.onPointerMove -= HandlePointerMove;
        InputController.onPanStart -= HandlePanStart;
        InputController.onPanEnd -= HandlePanEnd;
        InputController.onOrbitStart -= HandleOrbitStart;
        InputController.onOrbitEnd -= HandleOrbitEnd;
    }

    private void HandlePanStart() {
        panning = true;
    }

    private void HandlePanEnd() {
        panning = false;
    }

    private void HandleOrbitStart() {
        orbiting = true;
    }

    private void HandleOrbitEnd() {
        orbiting = false;
    }

    private void HandlePointerMove(Vector2 position, Vector2 delta) {
        HandlePan(delta);
        HandleOrbit(delta);
    }

    private void HandlePan(Vector2 delta) {
        // TODO: smoothing mouse input
        // TODO: raycast to scene to determine drag delta
        if (panning) {
            Plane groundPlane = new Plane(Vector3.up, 0);
            float panSpeed = 0.025f;
            Vector3 panDelta = delta.x * GetPanRight() + delta.y * GetPanForward();
            cameraAnchorTransform.Translate(-panSpeed * panDelta, Space.World);
        }
    }

    private void HandleOrbit(Vector2 delta) {
        // TODO: x axis rotation limits
        if (orbiting) {
            float orbitSpeed = 0.1f;
            Vector3 orbitDelta = orbitSpeed * new Vector3(-delta.y, delta.x, 0);
            cameraAnchorTransform.eulerAngles += orbitDelta;
            Mathf.Clamp(cameraAnchorTransform.eulerAngles.x, 0, 90);
        }
    }

    private void HandleZoom() {

    }

    private Vector3 GetPanForward() {
        float angle = cameraAnchorTransform.rotation.eulerAngles.y;
        return Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }

    private Vector3 GetPanRight() {
        float angle = cameraAnchorTransform.rotation.eulerAngles.y;
        return Quaternion.Euler(0, angle, 0) * Vector3.right;
    }
}

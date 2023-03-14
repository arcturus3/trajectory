using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Waypoints : MonoBehaviour
{
    public GameObject waypointMarker;
    private GameObject activeWaypoint;
    private List<GameObject> waypoints;
    private DropStage dropStage;
    private Vector3 dropPoint;
    private bool canDropHorizontal;
    private Camera camera;

    private void Start() {
        camera = Camera.main;
    }

    private void OnEnable() {
        InputController.onInputModeChange += HandleInputModeChange;
        InputController.onMove += HandleMove;
        InputController.onDrop += HandleDrop;
    }

    private void OnDisable() {
        InputController.onInputModeChange -= HandleInputModeChange;
        InputController.onMove -= HandleMove;
        InputController.onDrop -= HandleDrop;
    }

    private void HandleInputModeChange(InputMode inputMode) {
        switch (inputMode) {
            case InputMode.Edit:
                HandleEnterEditMode();
                break;
            case InputMode.Navigate:
                HandleExitEditMode();
                break;
        }
    }

    private void HandleEnterEditMode() {
        activeWaypoint = Instantiate(waypointMarker);
    }

    private void HandleExitEditMode() {
        Destroy(activeWaypoint);
    }

    private void HandleMove(Vector2 position) {
        Ray ray = camera.ScreenPointToRay(position);
        switch (dropStage) {
            case DropStage.Horizontal:
                RaycastHit hit;
                bool success = Physics.Raycast(ray, out hit);
                canDropHorizontal = success;
                if (success) {
                    dropPoint = hit.point;
                    activeWaypoint.transform.position = hit.point;
                    activeWaypoint.SetActive(true);
                }
                else {
                    activeWaypoint.SetActive(false);
                }
                break;
            case DropStage.Vertical:
                Vector3 point1 = camera.transform.position;
                Vector3 point2 = camera.transform.position + camera.transform.right;
                Vector3 point3 = camera.transform.position + ray.direction;
                Plane plane = new Plane(point1, point2, point3);
                Ray dropRay = new Ray(dropPoint + 1000 * Vector3.down, Vector3.up);
                float dist;
                plane.Raycast(dropRay, out dist);
                dropPoint = dropRay.GetPoint(dist);
                break;
        }
    }

    private void HandleDrop(Vector2 position) {
        switch (dropStage) {
            case DropStage.Horizontal:
                HandleDropHorizontal(position);
                break;
            case DropStage.Vertical:
                HandleDropVertical(position);
                break;
        }
    }

    private void HandleDropHorizontal(Vector2 position) {
        if (canDropHorizontal) {
            dropStage = DropStage.Vertical;
        }
    }

    private void HandleDropVertical(Vector2 position) {
        GameObject waypoint = Instantiate(waypointMarker);
        waypoint.transform.position = dropPoint;
        waypoints.Add(waypoint);
        dropStage = DropStage.Horizontal;
    }

    private enum DropStage {
        Horizontal,
        Vertical,
    }
}

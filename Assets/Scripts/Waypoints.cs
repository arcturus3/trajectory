using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static event Action<List<Vector3>> onWaypointsChange;
    public GameObject waypointMarker;

    private bool active;
    private GameObject activeWaypoint;
    private List<GameObject> waypoints;
    private DropStage dropStage;
    private Vector3 dropPoint;
    private bool canDropHorizontal;
    private Camera camera;

    private void Start() {
        active = false;
        activeWaypoint = null;
        waypoints = new List<GameObject>();
        dropStage = DropStage.Horizontal;
        dropPoint = Vector3.zero;
        canDropHorizontal = false;
        camera = Camera.main;
    }

    private void OnEnable() {
        InputController.onInputModeChange += HandleInputModeChange;
        InputController.onPointerMove += HandleMove;
        InputController.onWaypointDrop += HandleDrop;
    }

    private void OnDisable() {
        InputController.onInputModeChange -= HandleInputModeChange;
        InputController.onPointerMove -= HandleMove;
        InputController.onWaypointDrop -= HandleDrop;
    }

    private void HandleInputModeChange(InputMode inputMode) {
        switch (inputMode) {
            case InputMode.Edit:
                activeWaypoint = Instantiate(waypointMarker);
                active = true;
                break;
            case InputMode.Navigate:
                Destroy(activeWaypoint);
                active = false;
                break;
        }
    }

    private void HandleMove(Vector2 position, Vector2 delta) {
        if (active) {
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
                    activeWaypoint.transform.position = dropPoint;
                    break;
            }
        }
    }

    private void HandleDrop() {
        switch (dropStage) {
            case DropStage.Horizontal:
                if (canDropHorizontal) {
                    dropStage = DropStage.Vertical;
                }
                break;
            case DropStage.Vertical:
                GameObject waypoint = Instantiate(waypointMarker);
                waypoint.transform.position = dropPoint;
                waypoints.Add(waypoint);
                dropStage = DropStage.Horizontal;
                onWaypointsChange.Invoke(waypoints.Select(
                    waypoint => waypoint.transform.position
                ).ToList());
                break;
        }
    }

    private enum DropStage {
        Horizontal,
        Vertical,
    }
}
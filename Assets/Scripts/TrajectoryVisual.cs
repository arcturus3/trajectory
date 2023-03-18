using UnityEngine;
using System.Collections.Generic;

public class TrajectoryVisual : MonoBehaviour {
    LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        Waypoints.onWaypointsChange += HandleWaypointsChange;
    }

    private void OnDisable() {
        Waypoints.onWaypointsChange -= HandleWaypointsChange;
    }

    private void HandleWaypointsChange(List<Vector3> waypoints) {
        lineRenderer.positionCount = waypoints.Count;
        lineRenderer.SetPositions(waypoints.ToArray());
    }
}
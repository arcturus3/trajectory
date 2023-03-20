using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour {
    LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();

        Client client = new Client();
        Waypoint initialWaypoint = new Waypoint(new Vector3(0, 0, 1));
        Waypoint finalWaypoint = new Waypoint(new Vector3(10, 0, 1));
        // Waypoint finalWaypoint = new Waypoint(new Vector3(2, 0, 1));
        List<Waypoint> waypoints = new List<Waypoint>() {initialWaypoint, finalWaypoint};
        Trajectory trajectory = new Trajectory(client, waypoints);
        
        // Vector3 initialPosition = trajectory.GetPosition(0);
        // Vector3 finalPosition = trajectory.GetPosition(5);
        // List<Vector3> positions = new List<Vector3>() {initialPosition, finalPosition};
        // HandleWaypointsChange(positions);

        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i <= 100; i++) {
            float time = 5f * i / 100f;
            Vector3 position = trajectory.GetPose(time).Item1;
            positions.Add(position);
        }
        HandleWaypointsChange(positions);
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
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracker : MonoBehaviour
{
    public Trajectory trajectory;
    public static event Action onTrackingComplete;

    private bool tracking;
    private float startTime;

    private void Start() {
        Client client = new Client();
        Waypoint initialWaypoint = new Waypoint(new Vector3(0, 0, 1));
        Waypoint finalWaypoint = new Waypoint(new Vector3(10, 0, 1));
        // Waypoint finalWaypoint = new Waypoint(new Vector3(2, 0, 1));
        List<Waypoint> waypoints = new List<Waypoint>() {initialWaypoint, finalWaypoint};
        trajectory = new Trajectory(client, waypoints);

        tracking = false;
        startTime = 0;
        GetComponent<Renderer>().enabled = false;
    }

    private void OnEnable() {
        UI.onTrackingStart += StartTracking;
        UI.onTrackingStop += StopTracking;
    }

    private void OnDisable() {
        UI.onTrackingStart -= StartTracking;
        UI.onTrackingStop -= StopTracking;
    }

    private void StartTracking() {
        tracking = true;
        startTime = Time.time;
        GetComponent<Renderer>().enabled = true;
    }

    private void StopTracking() {
        tracking = false;
        GetComponent<Renderer>().enabled = false;
    }

    private void Update() {
        if (tracking) {
            float time = Time.time - startTime;
            if (time > trajectory.GetDuration()) {
                StopTracking();
                onTrackingComplete?.Invoke();
            }
            else {
                (Vector3 position, Vector3 normal) = trajectory.GetPose(time);
                transform.position = position;
                transform.up = normal;
            }
        }
    }
}

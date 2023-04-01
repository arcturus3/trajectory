using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryEditor : MonoBehaviour {
    public List<Vector3> constraints;
    public Trajectory trajectory;
    Client client;

    private void Start() {
        constraints = new List<Vector3>();
        client = new Client();
    }

    private void OnEnable() {
        Waypoints.onWaypointsChange += HandleConstraintsChange;
    }

    private void OnDisable() {
        Waypoints.onWaypointsChange -= HandleConstraintsChange;
    }

    private void HandleConstraintsChange(List<Vector3> constraints) {
        this.constraints = constraints;
    }


}
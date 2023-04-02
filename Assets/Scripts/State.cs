using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class State {
    private static State instance;
    private Client client;
    private List<Vector3> constraints;
    private Trajectory trajectory;

    public static State Instance {
        get {
            if (instance is null) {
                instance = new State();
            }
            return instance;
        }
    }

    public Trajectory Trajectory {
        get {return trajectory;}
    }

    public event Action TrajectoryChange;

    private State() {
        client = new Client();
        constraints = new List<Vector3>();
        trajectory = null;
    }

    public void SetConstraints(List<Vector3> constraints) {
        const float segmentDuration = 5;
        this.constraints = constraints;
        if (constraints.Count > 1) {
            List<Waypoint> waypoints = new List<Waypoint>();
            for (int i = 0; i < constraints.Count; i++) {
                waypoints.Add(new Waypoint() {
                    Position = constraints[i],
                    Time = segmentDuration * i
                });
            }
            // trajectory = new Trajectory(client, waypoints);
        }
        else {
            trajectory = null;
        }
        TrajectoryChange?.Invoke();
    }
}
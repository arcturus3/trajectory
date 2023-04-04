using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {
    private Client client;
    private List<Constraint> constraints;

    public event Action Generated;

    private void Start() {
        client = new Client();
    }

    public void Generate() {
        constraints = GetComponentsInChildren<Constraint>().ToList();
        bool feasible = client.GenerateTrajectory(constraints);
        if (feasible) {
            Generated?.Invoke();
        }
        else {
            Debug.LogWarning("Trajectory is infeasible");
        }
    }

    public (Vector3, Vector3) GetPose(float time) {
        return client.QueryTrajectory(time);
    }

    public float GetDuration() {
        return constraints.Last().Time;
    }
}

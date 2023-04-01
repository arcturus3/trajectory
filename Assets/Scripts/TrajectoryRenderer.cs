using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour {
    private LineRenderer lineRenderer;
    private const float pointsPerSecond = 10;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable() {
        State.Instance.TrajectoryChange += HandleTrajectoryChange;
    }

    private void OnDisable() {
        State.Instance.TrajectoryChange -= HandleTrajectoryChange;
    }

    private void HandleTrajectoryChange() {
        float duration = State.Instance.Trajectory.GetDuration();
        Debug.Log("HandleTrajectoryChange");
        Debug.Log(duration);
        int points = Mathf.RoundToInt(duration * pointsPerSecond);
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < points; i++) {
            float time = duration * i / (points - 1);
            Vector3 position = State.Instance.Trajectory.GetPose(time).Item1;
            positions.Add(position);
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
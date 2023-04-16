using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour {
    private Trajectory trajectory;
    private LineRenderer lineRenderer;
    private const float pointsPerSecond = 10;

    private void Start() {
        trajectory = GetComponent<Trajectory>();
        lineRenderer = GetComponent<LineRenderer>();
        trajectory.Generated += Render;
    }

    private void Render() {
        float duration = trajectory.GetDuration();
        int points = Mathf.RoundToInt(duration * pointsPerSecond);
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < points; i++) {
            float time = duration * i / (points - 1);
            Vector3 position = trajectory.GetPose(time).Item1;
            positions.Add(position);
        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
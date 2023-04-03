using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrajectoryTracker))]
public class TrajectoryTrackerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        TrajectoryTracker trajectoryTracker = (TrajectoryTracker) target;
        if (trajectoryTracker.Tracking) {
            if (GUILayout.Button("Stop")) {
                trajectoryTracker.StopTracking();
            }
        }
        else {
            if (GUILayout.Button("Start")) {
                trajectoryTracker.StartTracking();
            }
        }
    }
}

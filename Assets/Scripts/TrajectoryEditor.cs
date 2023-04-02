using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trajectory))]
public class TrajectoryEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Trajectory trajectory = (Trajectory) target;
        if (GUILayout.Button("Generate")) {
            trajectory.Generate();
        }
    }
}
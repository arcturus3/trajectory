using System;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracker : MonoBehaviour
{
    [SerializeField]
    private Trajectory trajectory;
    private float startTime;
    public bool Tracking {get; private set;}

    private void Start() {
        Tracking = false;
        startTime = 0;
        SetVisible(false);
    }

    public void StartTracking() {
        Tracking = true;
        startTime = Time.time;
        SetVisible(true);
    }

    public void StopTracking() {
        Tracking = false;
        SetVisible(false);
    }

    private void Update() {
        if (Tracking) {
            float time = Time.time - startTime;
            Debug.Log(time);
            Debug.Log(trajectory.GetPose(time));
            if (time > trajectory.GetDuration()) {
                StopTracking();
            }
            else {
                (Vector3 position, Vector3 normal) = trajectory.GetPose(time);
                transform.position = position;
                transform.up = normal;
            }
        }
    }

    private void SetVisible(bool visible) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(visible);
        }
    }
}

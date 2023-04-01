using System;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryTracker : MonoBehaviour
{
    public static event Action onTrackingComplete;

    private bool tracking;
    private float startTime;

    private void Start() {
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
            if (time > State.Instance.Trajectory.GetDuration()) {
                StopTracking();
                onTrackingComplete?.Invoke();
            }
            else {
                (Vector3 position, Vector3 normal) = State.Instance.Trajectory.GetPose(time);
                transform.position = position;
                transform.up = normal;
            }
        }
    }
}

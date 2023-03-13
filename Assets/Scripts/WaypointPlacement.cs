using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPlacement : MonoBehaviour {

    private Plane groundPlane;
    private GameObject sphere;
    private bool placingGround;
    private Vector3 groundPoint;

    void Start() {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    void Update() {
        if (placingGround) {
            HandleGroundPlace();
        }
        else {
            HandleAirPlace();
        }
    }

    void HandleGroundPlace() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0;
        bool hit = groundPlane.Raycast(ray, out enter);
        if (hit) {
            Vector3 groundPoint = ray.GetPoint(enter);
            // float snappedX = Mathf.Round(groundPoint.x * 1) / 1;
            // float snappedZ = Mathf.Round(groundPoint.z * 1) / 1;
            // Vector3 snappedPoint = new Vector3(snappedX, 0, snappedZ);
            // sphere.transform.position = snappedPoint;
            sphere.transform.position = groundPoint;
        }
        if (Input.GetMouseButtonDown(0)) {
            placingGround = false;
            groundPoint = groundPoint;

        }
    }

    void HandleAirPlace() {
        
    }
}
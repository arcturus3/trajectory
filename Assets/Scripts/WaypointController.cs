using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaypointController : MonoBehaviour
{
    public GameObject marker;
    private GameObject waypointPreview;
    private List<GameObject> waypoints;

    private GameObject activeWaypoint;
    private bool settingXZ;
    private bool settingY;

    // Start is called before the first frame update
    void Start()
    {
        waypointPreview = Instantiate(marker);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Mouse.current.position.value;
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        bool success = Physics.Raycast(ray, out hit);
        if (success) {
            waypointPreview.SetActive(true);
            waypointPreview.transform.position = hit.point;
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                GameObject waypoint = Instantiate(marker);
                waypoint.transform.position = hit.point;
                waypoints.Add(waypoint);
            }
        }
        else {
            waypointPreview.SetActive(false);
        }
    }
}

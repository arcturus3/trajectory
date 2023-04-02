using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class Trajectory : MonoBehaviour {
    private Client client;
    private int id;
    private List<Constraint> constraints;

    public event Action Generated;

    private void Awake() {
        client = new Client();
    }

    public void Generate() {
        constraints = GetComponentsInChildren<Constraint>().ToList();
        List<MessageWaypoint> messageWaypoints = new List<MessageWaypoint>();
        foreach (Constraint constraint in constraints) {
            messageWaypoints.Add(new MessageWaypoint() {
                Position = ConvertPointRequest(constraint.gameObject.transform.position),
                Time = constraint.Time
            });
        }
        GenerateRequest request = new GenerateRequest() {
            Waypoints = messageWaypoints.ToList()
        };
        string requestString = JsonSerializer.Serialize(request);
        string responseString = client.SendRequest(requestString);
        GenerateResponse response = JsonSerializer.Deserialize<GenerateResponse>(responseString);
        id = response.TrajectoryId;
        Debug.Log(constraints.Last());
        Generated?.Invoke();
    }

    public (Vector3, Vector3) GetPose(float time) {
        QueryRequest request = new QueryRequest() {
            TrajectoryId = id,
            Time = time
        };
        string requestString = JsonSerializer.Serialize(request);
        string responseString = client.SendRequest(requestString);
        QueryResponse response = JsonSerializer.Deserialize<QueryResponse>(responseString);
        return (ConvertPointResponse(response.Position), ConvertPointResponse(response.Normal));
    }

    public float GetDuration() {
        return constraints.Last().Time;
    }

    private Vector3 ConvertPointResponse(List<float> point) {
        float x = point[0];
        float y = point[1];
        float z = point[2];
        return new Vector3(x, z, y);
    }

    private List<float> ConvertPointRequest(Vector3 point) {
        return new List<float>() {point.x, point.z, point.y};
    }
}

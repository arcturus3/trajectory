using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using UnityEngine;

public class Trajectory {
    private Client client;
    private List<Waypoint> waypoints;
    private int id;

    public Trajectory(Client client, List<Waypoint> waypoints) {
        this.client = client;
        this.waypoints = waypoints;
        List<MessageWaypoint> messageWaypoints = new List<MessageWaypoint>();
        foreach (Waypoint waypoint in waypoints) {
            messageWaypoints.Add(new MessageWaypoint() {
                Position = ConvertPointRequest(waypoint.Position),
                Time = waypoint.Time
            });
        }
        GenerateRequest request = new GenerateRequest() {
            Waypoints = messageWaypoints.ToList()
        };
        string requestString = JsonSerializer.Serialize(request);
        string responseString = client.SendRequest(requestString);
        GenerateResponse response = JsonSerializer.Deserialize<GenerateResponse>(responseString);
        id = response.TrajectoryId;
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
        return waypoints.Last().Time;
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

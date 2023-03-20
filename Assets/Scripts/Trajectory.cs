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
        GenerateRequest request = new GenerateRequest() {
            Waypoints = waypoints.Select(waypoint => new MessageWaypoint{
                Position = new List<float>() {
                    waypoint.position.x,
                    waypoint.position.y,
                    waypoint.position.z
                }
            }).ToList()
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
        return (ConvertPoint(response.Position), ConvertPoint(response.Normal));
    }

    public float GetDuration() {
        return 5f;
    }

    private Vector3 ConvertPoint(List<float> point) {
        float x = point[0];
        float y = point[1];
        float z = point[2];
        return new Vector3(x, z, y);
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class GenerateRequest
{
    [JsonPropertyName("message_type")]
    public string MessageType {get;} = "generate_request";
    [JsonPropertyName("waypoints")]
    public List<MessageWaypoint> Waypoints {get; set;}
}

public class GenerateResponse {
    [JsonPropertyName("message_type")]
    public string MessageType {get;} = "generate_response";
    [JsonPropertyName("trajectory_id")]
    public int TrajectoryId {get; set;}
    [JsonPropertyName("feasible")]
    public bool Feasible {get; set;}
}

public class QueryRequest {
    [JsonPropertyName("message_type")]
    public string MessageType {get;} = "query_request";
    [JsonPropertyName("trajectory_id")]
    public int TrajectoryId {get; set;}
    [JsonPropertyName("time")]
    public float Time {get; set;}
}

public class QueryResponse {
    [JsonPropertyName("message_type")]
    public string MessageType {get;} = "query_response";
    [JsonPropertyName("position")]
    public List<float> Position {get; set;}
    [JsonPropertyName("normal")]
    public List<float> Normal {get; set;}
}

public class MessageWaypoint {
    [JsonPropertyName("position")]
    public List<float> Position {get; set;}
    [JsonPropertyName("rotation")]
    public List<float> Rotation {get; set;}
    [JsonPropertyName("time")]
    public float Time {get; set;}
    [JsonPropertyName("constrain_rotation")]
    public bool ConstrainRotation {get; set;}
}
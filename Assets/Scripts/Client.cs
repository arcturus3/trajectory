using System;
using NetMQ;
using NetMQ.Sockets;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Client : IDisposable {
    RequestSocket socket;
    // not using multiple trajectories for simplicity, just reference most recent
    private int trajectoryId;
    private bool debug;

    public Client() {
        socket = new RequestSocket();
        socket.Connect("tcp://localhost:5555");
        trajectoryId = 0;
        debug = false;
    }

    public void Dispose() {
        socket.Close();
    }

    private string SendRequest(string request) {
        socket.SendFrame(request);
        if (debug) Debug.Log($"sent: {request}");
        string response = socket.ReceiveFrameString();
        if (debug) Debug.Log($"received: {response}");
        return response;
    }

    public bool GenerateTrajectory(List<Constraint> constraints) {
        List<ConstraintMessage> constraintMessages = constraints.Select(
            constraint => {
                Vector3 position = ConvertPosition(constraint.Position);
                Matrix4x4 rotation = ConvertRotation(Matrix4x4.Rotate(constraint.Rotation));
                return new ConstraintMessage() {
                    Position = SerializePosition(position),
                    Rotation = SerializeRotation(rotation),
                    Time = constraint.Time,
                    ConstrainRotation = constraint.ConstrainRotation
                };
            }
        ).ToList();
        GenerateRequest request = new GenerateRequest() {
            Constraints = constraintMessages
        };
        string requestString = JsonSerializer.Serialize(request);
        string responseString = SendRequest(requestString);
        GenerateResponse response = JsonSerializer.Deserialize<GenerateResponse>(responseString);
        trajectoryId = response.TrajectoryId;
        return response.Feasible;
    }

    public (Vector3, Vector3) QueryTrajectory(float time) {
        QueryRequest request = new QueryRequest() {
            TrajectoryId = trajectoryId,
            Time = time
        };
        string requestString = JsonSerializer.Serialize(request);
        string responseString = SendRequest(requestString);
        QueryResponse response = JsonSerializer.Deserialize<QueryResponse>(responseString);
        Vector3 position = ConvertPosition(DeserializePosition(response.Position));
        Vector3 normal = ConvertPosition(DeserializePosition(response.Normal));
        return (position, normal);
    }

    private List<float> SerializePosition(Vector3 position) {
        return new List<float>() {position.x, position.y, position.z};
    }

    private Vector3 DeserializePosition(List<float> position) {
        return new Vector3(position[0], position[1], position[2]);
    }

    private List<List<float>> SerializeRotation(Matrix4x4 rotation) {
        return new List<List<float>>() {
            SerializePosition(rotation.GetRow(0)),
            SerializePosition(rotation.GetRow(1)),
            SerializePosition(rotation.GetRow(2))
        };
    }

    private Matrix4x4 DeserializeRotation(List<List<float>> rotation) {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, DeserializePosition(rotation[0]));
        matrix.SetRow(1, DeserializePosition(rotation[1]));
        matrix.SetRow(2, DeserializePosition(rotation[2]));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));
        return matrix;
    }

    private Vector3 ConvertPosition(Vector3 position) {
        return GetCoordsMatrix() * new Vector4(position.x, position.y, position.z, 1);
    }

    private Matrix4x4 ConvertRotation(Matrix4x4 rotation) {
        return GetCoordsMatrix() * rotation * GetCoordsMatrix();
    }

    // return change of basis matrix W, W = W^(-1)
    private Matrix4x4 GetCoordsMatrix() {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(1, 0, 0, 0));
        matrix.SetRow(1, new Vector4(0, 0, 1, 0));
        matrix.SetRow(2, new Vector4(0, 1, 0, 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));
        return matrix;
    }
}

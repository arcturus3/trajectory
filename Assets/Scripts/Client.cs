using System;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class Client : IDisposable {
    RequestSocket socket;

    public Client() {
        socket = new RequestSocket();
        socket.Connect("tcp://localhost:5555");
    }

    public void Dispose() {
        socket.Close();
    }

    public string SendRequest(string request) {
        socket.SendFrame(request);
        Debug.Log($"sent: {request}");
        string response = socket.ReceiveFrameString();
        Debug.Log($"received: {response}");
        return response;
    }
}

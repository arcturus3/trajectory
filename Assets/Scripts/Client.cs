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

    public void Request() {
        Debug.Log("sending request...");
        socket.SendFrame("hello!");
        string response = socket.ReceiveFrameString();
        Debug.Log($"received response: {response}");
    }
}

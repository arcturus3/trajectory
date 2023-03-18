using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MQDebug : MonoBehaviour
{
    private void Start() {
        Client client = new Client();
        client.Request();
    }
}

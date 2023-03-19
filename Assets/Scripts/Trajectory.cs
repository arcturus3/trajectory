using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

public class Trajectory {
    private Client client;
    private List<Waypoint> waypoints;

    public Trajectory(Client client, List<Waypoint> waypoints) {
        this.client = client;
        this.waypoints = waypoints;
        Dictionary<
        client.SendRequest(?);
        /*
        {
            command: 'generate',
            args: {
                waypoints: [
                    {position: [x, y, z]},
                    {position: [x, y, z]},
                    ...
                ]
            }
        }
        {
            trajectory_id: x
        }

        {
            command: 'query',
            args: {
                trajectory_id: x,
                time: x
            }
        }
        {
            position: [x, y, z],
            normal: [x, y, z]
        }
        */
    }

    public Vector3 GetPosition(float time) {
        {
            'get_position',

        }
        client.SendRequest(?);
    }

    public Quaternion GetRotation(float time) {
        client.SendRequest(?);
    }
}
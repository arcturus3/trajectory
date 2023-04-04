using UnityEngine;

public class Constraint : MonoBehaviour {
    public Vector3 Position {
        get {return transform.position;}
    }
    public Quaternion Rotation {
        get {return transform.rotation;}
    }
    [field: SerializeField]
    public float Time {get; private set;}
    [field: SerializeField]
    public bool ConstrainRotation {get; private set;}
}

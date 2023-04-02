using UnityEngine;

public class Constraint : MonoBehaviour {
    [field: SerializeField]
    public float Time {get; set;}

    private void Start() {
        Time = 0;
    }
}
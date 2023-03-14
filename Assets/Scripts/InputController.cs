using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputController : MonoBehaviour
{
    public static event Action<InputMode> onInputModeChange;
    public static event Action<Vector2, Vector2> onPointerMove;
    public static event Action onPanStart;
    public static event Action onPanEnd;
    public static event Action onOrbitStart;
    public static event Action onOrbitEnd;
    public static event Action onWaypointDrop;

    private InputMode inputMode;

    private void OnEnable() {
        UI.onInputModeChange += SetInputMode;
    }

    private void OnDisable() {
        UI.onInputModeChange -= SetInputMode;
    }

    private void Update() {
        onPointerMove.Invoke(
            Mouse.current.position.value,
            Mouse.current.delta.value
        );
        switch (inputMode) {
            case InputMode.Navigate:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    onPanStart.Invoke();
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    onPanEnd.Invoke();
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    onOrbitStart.Invoke();
                if (Mouse.current.rightButton.wasReleasedThisFrame)
                    onOrbitEnd.Invoke();
                break;
            case InputMode.Edit:
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    onWaypointDrop.Invoke();
                break;
        }
    }

    private void SetInputMode(InputMode mode) {
        inputMode = mode;
        onInputModeChange.Invoke(mode);
    }
}

public enum InputMode {
    Navigate,
    Edit
}
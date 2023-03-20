using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour {
    private InputMode inputMode = InputMode.Navigate;
    private bool tracking = false;

    private Button navigateButton;
    private Button editButton;
    private Button trackingButton;

    public static event Action<InputMode> onInputModeChange;
    public static event Action onTrackingStart;
    public static event Action onTrackingStop;

    private void OnEnable() {
        UIDocument document = GetComponent<UIDocument>();
        navigateButton = document.rootVisualElement.Q("navigate-button") as Button;
        editButton = document.rootVisualElement.Q("edit-button") as Button;
        trackingButton = document.rootVisualElement.Q("tracking-button") as Button;
        navigateButton.RegisterCallback<ClickEvent>(HandleClick);
        editButton.RegisterCallback<ClickEvent>(HandleClick);
        trackingButton.clicked += HandleTrackingButtonClick;
        SetInputMode(InputMode.Navigate);
    }

    private void OnDisable() {
        navigateButton.UnregisterCallback<ClickEvent>(HandleClick);
        editButton.UnregisterCallback<ClickEvent>(HandleClick);
        trackingButton.clicked -= HandleTrackingButtonClick;
    }

    private void HandleClick(ClickEvent clickEvent) {
        Button target = clickEvent.target as Button;
        SetInputMode(GetInputMode(target));
    }

    private void HandleTrackingButtonClick() {
        if (tracking) {
            tracking = false;
            trackingButton.text = "Start";
            onTrackingStop?.Invoke();
        }
        else {
            tracking = true;
            trackingButton.text = "Stop";
            onTrackingStart?.Invoke();
        }
    }

    private void SetInputMode(InputMode newInputMode) {
        GetButton(inputMode).RemoveFromClassList("selected");
        GetButton(newInputMode).AddToClassList("selected");
        inputMode = newInputMode;
        onInputModeChange?.Invoke(inputMode);
    }

    private Button GetButton(InputMode inputMode) {
        switch (inputMode) {
            case InputMode.Navigate:
                return navigateButton;
            case InputMode.Edit:
                return editButton;
            default:
                return null;
        }
    }

    private InputMode GetInputMode(Button button) {
        switch (button.name) {
            case "navigate-button":
                return InputMode.Navigate;
            case "edit-button":
                return InputMode.Edit;
            default:
                return InputMode.Navigate;
        }
    }
}

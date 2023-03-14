using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour {
    private InputMode inputMode = InputMode.Navigate;
    private Button navigateButton;
    private Button editButton;

    public static event Action<InputMode> onInputModeChange;

    private void OnEnable() {
        UIDocument document = GetComponent<UIDocument>();
        navigateButton = document.rootVisualElement.Q("navigate-button") as Button;
        editButton = document.rootVisualElement.Q("edit-button") as Button;
        navigateButton.RegisterCallback<ClickEvent>(HandleClick);
        editButton.RegisterCallback<ClickEvent>(HandleClick);
        SetInputMode(InputMode.Navigate);
    }

    private void OnDisable() {
        navigateButton.UnregisterCallback<ClickEvent>(HandleClick);
        editButton.UnregisterCallback<ClickEvent>(HandleClick);
    }

    private void HandleClick(ClickEvent clickEvent) {
        Button target = clickEvent.target as Button;
        SetInputMode(GetInputMode(target));
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

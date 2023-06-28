using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewInputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, GenericInput.IGenericMapActions
{
    public Dictionary<InputActionPhase, Action> ClickEvents = new()
    {
        { InputActionPhase.Started, null },
        { InputActionPhase.Performed, null },
        { InputActionPhase.Canceled, null }
    };
    public Dictionary<InputActionPhase, Action> EscapeEvents = new()
    {
        { InputActionPhase.Started, null },
        { InputActionPhase.Performed, null },
        { InputActionPhase.Canceled, null }
    };

    public void OnLeftClick(InputAction.CallbackContext context) => ClickEvents[context.phase]?.Invoke();

    public void OnEscape(InputAction.CallbackContext context) => EscapeEvents[context.phase]?.Invoke();
}

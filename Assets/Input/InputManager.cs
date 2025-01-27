using System;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-99)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    private InputControl inputControl;
    private InputAction draggingAction;
    private InputAction manupilationAction;
    public event Action<bool> OnDraggingAction;
    public event Action<Vector2> OnDraggingInput;

    public Vector2 draggingDirection;
    private void Awake()
    {
        Instance = this;
        inputControl = new InputControl();
        inputControl.Player.Enable();

        draggingAction = inputControl.Player.Dragging;
        draggingAction.Enable();
        draggingAction.performed += DraggingActionOnperformed;


        manupilationAction = inputControl.Player.Manipulation;
        manupilationAction.Enable();
        manupilationAction.started += context => { OnDraggingAction?.Invoke(true); };
        manupilationAction.canceled += context => { OnDraggingAction?.Invoke(false); };

    }

    private void OnDestroy()
    {
        draggingAction.performed -= DraggingActionOnperformed;
    }

    private void DraggingActionOnperformed(InputAction.CallbackContext obj)
    {
        draggingDirection = draggingAction.ReadValue<Vector2>();
    }
}
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
   [SerializeField] private bool isTrigger;
   [SerializeField] private Vector2 touchPosition;
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
        manupilationAction.started += context => { isTrigger = true; };
        manupilationAction.canceled += context => { isTrigger = false;};

    }

    private void OnDestroy()
    {
        draggingAction.performed -= DraggingActionOnperformed;
    }

    private void DraggingActionOnperformed(InputAction.CallbackContext obj)
    {
        touchPosition = draggingAction.ReadValue<Vector2>();
    }

    public Vector3 GetTouchPosition()
    {
        return touchPosition;
    }

    public bool IsTrigger()
    {
        return isTrigger;
    }
}
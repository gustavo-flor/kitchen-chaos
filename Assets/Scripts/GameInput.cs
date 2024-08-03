using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction; 
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += OnInteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += OnInteractAlternatePerformed;
    }

    private void OnInteractAlternatePerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private Vector3 GetMovementVector()
    {
        var move = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(move.x, 0, move.y);
    }
    
    public Vector3 GetMovementVectorNormalized()
    {
        return GetMovementVector().normalized;
    }
}

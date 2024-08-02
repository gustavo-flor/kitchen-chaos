using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnInteractAction;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += OnInteractPerformed;
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

using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
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

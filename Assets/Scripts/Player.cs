using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool _isWalking;
    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var movement = gameInput.GetMovementVectorNormalized();
        _isWalking = movement != Vector3.zero;
        if (_isWalking)
        {
            transform.position += moveSpeed * Time.deltaTime * movement;
            transform.forward = Vector3.Slerp(transform.forward, movement, rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}

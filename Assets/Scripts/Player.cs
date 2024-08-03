using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private const float PlayerRadius = .7f;
    private const float PlayerHeight = 2f;
    private const float InteractionDistance = 2f;

    private bool _isWalking;
    private Vector3 _lastInteractionMovement;
    private ClearCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    public event EventHandler<OnCounterSelectChangeEventArgs> OnCounterSelectChange;

    public class OnCounterSelectChangeEventArgs : EventArgs
    {
        public ClearCounter ClearCounter;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
            throw new SystemException();
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += OnInteractAction;
    }

    private void OnInteractAction(object sender, EventArgs e)
    {
        _selectedCounter?.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        var movement = gameInput.GetMovementVectorNormalized();
        if (movement != Vector3.zero)
        {
            _lastInteractionMovement = movement;
        }
        if (Physics.Raycast(transform.position, _lastInteractionMovement, out RaycastHit hit, InteractionDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                SetSelectedCounter(clearCounter);
                return;
            }
        }
        SetSelectedCounter(null);
    }

    private void HandleMovement()
    {
        var movement = gameInput.GetMovementVectorNormalized();
        _isWalking = movement != Vector3.zero;
        var canMove = CanMove(movement);
        if (!canMove)
        {
            var horizontalMovement = new Vector3(movement.x, 0f, 0f).normalized;
            canMove = CanMove(horizontalMovement);
            if (canMove)
            {
                movement = horizontalMovement;
            }
            else
            {
                var verticalMovement = new Vector3(0f, 0f, movement.z).normalized;
                canMove = CanMove(verticalMovement);
                if (canMove)
                {
                    movement = verticalMovement;
                }
            }
        }
        if (_isWalking && canMove)
        {
            transform.position += MoveDistance() * movement;
            transform.forward = Vector3.Slerp(transform.forward, movement, rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanMove(Vector3 direction)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, direction, MoveDistance());
    }

    private float MoveDistance()
    {
        return moveSpeed * Time.deltaTime;
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        _selectedCounter = clearCounter;
        OnCounterSelectChange?.Invoke(this, new OnCounterSelectChangeEventArgs { ClearCounter = clearCounter });
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}

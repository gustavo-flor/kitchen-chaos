using System;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public static KitchenObject Spawn(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        var kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }
        _kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("Counter already has a KitchenObject!");
            throw new SystemException();
        }
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }
    
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject plate)
        {
            plateKitchenObject = plate;
            return true;
        }
        plateKitchenObject = null;
        return false;
    }
    
    public bool TryAddIngredientOnPlate(KitchenObject kitchenObject)
    {
        var hasPlate = TryGetPlate(out var plateKitchenObject);
        if (!hasPlate)
        {
            return false;
        }
        var ingredientAdded = plateKitchenObject.TryAddIngredient(kitchenObject.GetKitchenObjectSO());
        if (ingredientAdded)
        {
            kitchenObject.DestroySelf();
        }
        return ingredientAdded;
    }
}

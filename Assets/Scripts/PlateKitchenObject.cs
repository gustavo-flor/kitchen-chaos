using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> _kitchenObjectSOs;

    private void Awake()
    {
        _kitchenObjectSOs = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!kitchenObjectSO.deliverableIngredient || _kitchenObjectSOs.Contains(kitchenObjectSO))
        {
            return false;
        }
        _kitchenObjectSOs.Add(kitchenObjectSO);
        return true;
    }
}

using System;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> _kitchenObjectSOs;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

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
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectSO = kitchenObjectSO });
        return true;
    }
}

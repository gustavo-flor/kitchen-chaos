using System;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> _ingredients;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO Ingredients;
    }

    private void Awake()
    {
        _ingredients = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!kitchenObjectSO.deliverableIngredient || _ingredients.Contains(kitchenObjectSO))
        {
            return false;
        }
        _ingredients.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { Ingredients = kitchenObjectSO });
        return true;
    }

    public List<KitchenObjectSO> GetIngredients()
    {
        return _ingredients;
    }
}

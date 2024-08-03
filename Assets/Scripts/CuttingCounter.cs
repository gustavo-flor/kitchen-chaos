using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private float _cuttingProgress;

    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            var kitchenObject = player.GetKitchenObject();
            if (HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
            {
                kitchenObject.SetKitchenObjectParent(this);
            }
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            _cuttingProgress = 0;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { ProgressNormalized = 0 });
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject())
        {
            return;
        }
        var kitchenObject = GetKitchenObject();
        var recipe = GetRecipeByKitchenObject(kitchenObject.GetKitchenObjectSO());
        if (recipe == null)
        {
            return;
        }
        _cuttingProgress++;
        var progressNormalized = _cuttingProgress / recipe.cuttingProgressMax;
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { ProgressNormalized = progressNormalized });
        if (_cuttingProgress < recipe.cuttingProgressMax)
        {
            return;
        }
        OnCut?.Invoke(this, EventArgs.Empty);
        kitchenObject.DestroySelf();
        KitchenObject.Spawn(recipe.output, this);
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObject)
    {
        return GetRecipeByKitchenObject(kitchenObject) != null;
    }
    
    private CuttingRecipeSO GetRecipeByKitchenObject(KitchenObjectSO kitchenObject)
    {
        return cuttingRecipeSOs.FirstOrDefault(cuttingRecipe => cuttingRecipe.input == kitchenObject);
    }
}

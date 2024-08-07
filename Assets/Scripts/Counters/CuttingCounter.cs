using System;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private float _cuttingProgress;

    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                return;
            }
            var kitchenObject = player.GetKitchenObject();
            if (HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
            {
                kitchenObject.SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                player.TryAddIngredientOnPlate(GetKitchenObject());
                return;
            }
            GetKitchenObject().SetKitchenObjectParent(player);
            _cuttingProgress = 0;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0 });
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
        if (recipe is null)
        {
            return;
        }
        _cuttingProgress++;
        var progressNormalized = _cuttingProgress / recipe.cuttingProgressMax;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = progressNormalized });
        OnCut?.Invoke(this, EventArgs.Empty);
        if (_cuttingProgress < recipe.cuttingProgressMax)
        {
            return;
        }
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

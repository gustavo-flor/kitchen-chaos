using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;

    private int cuttingProgress;
    
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
            cuttingProgress = 0;
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
        cuttingProgress++;
        if (cuttingProgress < recipe.cuttingProgressMax)
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

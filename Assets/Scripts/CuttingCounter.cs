using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOs;
    
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
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (!HasKitchenObject())
        {
            return;
        }
        var kitchenObject = GetKitchenObject();
        var output = GetOutputForInput(kitchenObject.GetKitchenObjectSO());
        if (output != null)
        {
            kitchenObject.DestroySelf();
            KitchenObject.Spawn(output, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObject)
    {
        return GetOutputForInput(kitchenObject) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        foreach (var cuttingRecipe in cuttingRecipeSOs)
        {
            if (cuttingRecipe.input == input)
            {
                return cuttingRecipe.output;
            }
        }
        return null;
    }
}

using System;
using System.Linq;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;

    private float _fryingTimer;
    private FryingRecipeSO _recipe;
    
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
            var recipe = GetRecipeByKitchenObject(kitchenObject.GetKitchenObjectSO());
            if (recipe is null)
            {
                return;
            }
            kitchenObject.SetKitchenObjectParent(this);
            _recipe = recipe;
            ResetFryingTimer();
        }
        else
        {
            if (player.HasKitchenObject())
            {
                player.TryAddIngredientOnPlate(GetKitchenObject());
                return;
            }
            GetKitchenObject().SetKitchenObjectParent(player);
            _recipe = null;
            ResetFryingTimer();
        }
    }
    
    private FryingRecipeSO GetRecipeByKitchenObject(KitchenObjectSO kitchenObject)
    {
        return fryingRecipeSOs.FirstOrDefault(cuttingRecipe => cuttingRecipe.input == kitchenObject);
    }

    private void Update()
    {
        if (!HasKitchenObject() || _recipe is null)
        {
            return;
        }
        var kitchenObject = GetKitchenObject();
        _fryingTimer += Time.deltaTime;
        if (_fryingTimer < _recipe.fryingTimerMax)
        {
            var progressNormalized = _fryingTimer / _recipe.fryingTimerMax;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = progressNormalized });
            return;
        }
        kitchenObject.DestroySelf();
        var output = _recipe.output;
        KitchenObject.Spawn(output, this);
        _recipe = GetRecipeByKitchenObject(output);
        ResetFryingTimer();
    }

    private void ResetFryingTimer()
    {
        _fryingTimer = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { ProgressNormalized = 0 });
    }
}

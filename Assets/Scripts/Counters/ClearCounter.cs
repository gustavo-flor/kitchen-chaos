public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                var ingredientAdded = player.TryAddIngredientOnPlate(GetKitchenObject());
                if (!ingredientAdded)
                {
                    GetKitchenObject().TryAddIngredientOnPlate(player.GetKitchenObject());
                }
                return;
            }
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }
}

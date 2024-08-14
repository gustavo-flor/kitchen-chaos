public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            var hasPlate = player.GetKitchenObject().TryGetPlate(out var plateKitchenObject);
            if (!hasPlate)
            {
                return;
            }
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            plateKitchenObject.DestroySelf();
        }
    }
}

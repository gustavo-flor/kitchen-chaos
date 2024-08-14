using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private float spawnRecipeTimerMax;
    [SerializeField] private int waitingRecipesMax;
    
    private List<RecipeSO> _waitingRecipeSOs;
    private float _spawnRecipeTimer;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOs = new List<RecipeSO>();
    }

    private void Update()
    {
        if (_waitingRecipeSOs.Count >= waitingRecipesMax)
        {
            return;
        }
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = spawnRecipeTimerMax;
            var waitingRecipe = recipeListSO.recipeSOs[Random.Range(0, recipeListSO.recipeSOs.Count)];
            _waitingRecipeSOs.Add(waitingRecipe);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        var recipeFound = false;
        foreach (var waitingRecipe in _waitingRecipeSOs)
        {
            var ingredients = plateKitchenObject.GetIngredients();
            var amountOfWaitingIngredients = waitingRecipe.kitchenObjectSOs.Count;
            if (amountOfWaitingIngredients != ingredients.Count)
            {
                continue;
            }
            var amountOfContainingIngredients = waitingRecipe.kitchenObjectSOs
                .Count(it => ingredients.Any(ingredient => it == ingredient));
            if (amountOfWaitingIngredients == amountOfContainingIngredients)
            {
                Debug.Log("Received a " + waitingRecipe.recipeName);
                _waitingRecipeSOs.Remove(waitingRecipe);
                recipeFound = true;
                break;
            }
        }
        if (!recipeFound)
        {
            Debug.Log("Player did not deliver a correct recipe");
        }
    }
}

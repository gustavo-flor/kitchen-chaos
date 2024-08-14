using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjects;
    
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += OnIngredientAdded;
        foreach (var map in kitchenObjectSOGameObjects)
        {
            map.gameObject.SetActive(false);
        }
    }

    private void OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var map in kitchenObjectSOGameObjects)
        {
            if (map.kitchenObjectSO == e.Ingredients)
            {
                map.gameObject.SetActive(true);
            }
        }
    }
}

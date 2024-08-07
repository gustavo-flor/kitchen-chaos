using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjects;

    private void Awake()
    {
        _plateVisualGameObjects = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += OnPlateSpawned;
        platesCounter.OnPlateRemoved += OnPlateRemoved;
    }
    
    private void OnPlateSpawned(object sender, EventArgs e)
    {
        var counterTopPoint = platesCounter.GetKitchenObjectFollowTransform();
        var plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        const float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * _plateVisualGameObjects.Count, 0);
        
        _plateVisualGameObjects.Add(plateVisualTransform.gameObject);
    }

    private void OnPlateRemoved(object sender, EventArgs e)
    {
        var plateGameObject = _plateVisualGameObjects[^1];
        _plateVisualGameObjects.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}

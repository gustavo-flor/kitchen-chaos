using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private float spawnPlateCountdown;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private int platesMaxAmount;
    
    private float _spawnPlateTimer;
    private int _platesAmount;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved; 
    
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && _platesAmount > 0)
        {
            _platesAmount--;
            KitchenObject.Spawn(plateKitchenObjectSO, player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > spawnPlateCountdown)
        {
            _spawnPlateTimer = 0;
            _platesAmount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}

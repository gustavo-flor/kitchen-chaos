using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;
    
    private void Start()
    {
        Player.Instance.OnCounterSelectChange += OnCounterSelectChange;
    }

    private void OnCounterSelectChange(object sender, Player.OnCounterSelectChangeEventArgs e)
    {
        foreach (var visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(baseCounter == e.BaseCounter);
        }
    }
}

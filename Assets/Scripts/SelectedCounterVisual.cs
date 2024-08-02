using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;
    
    private void Start()
    {
        Player.Instance.OnCounterSelectChange += OnCounterSelectChange;
    }

    private void OnCounterSelectChange(object sender, Player.OnCounterSelectChangeEventArgs e)
    {
        visualGameObject.SetActive(clearCounter == e.ClearCounter);
    }
}

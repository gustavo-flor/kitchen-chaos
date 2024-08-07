using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;
    
    private IHasProgress _hasProgress;

    private void Awake()
    {
        barImage.fillAmount = 0f;
    }

    private void Start()
    {
        _hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress is null)
        {
            Debug.LogError("Game object " + hasProgressGameObject + " doesn't have a component that implements IHasProgress!");
            throw new SystemException();
        }
        _hasProgress.OnProgressChanged += OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;
        var inProgress = barImage.fillAmount is not (0f or 1f);
        gameObject.SetActive(inProgress);
    }
}

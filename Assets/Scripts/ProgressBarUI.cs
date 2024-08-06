using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Awake()
    {
        barImage.fillAmount = 0f;
    }

    private void Start()
    {
        cuttingCounter.OnProgressChanged += OnProgressChanged;
        gameObject.SetActive(false);
    }

    private void OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;
        var inProgress = barImage.fillAmount is not (0f or 1f);
        gameObject.SetActive(inProgress);
    }
}

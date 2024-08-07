using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOn;
    [SerializeField] private GameObject particles;

    private void Start()
    {
        stoveCounter.OnProgressChanged += OnProgressChanged;
    }

    private void OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        var isFrying = e.ProgressNormalized is not (0f or 1f);
        stoveOn.SetActive(isFrying);
        particles.SetActive(isFrying);
    }
}

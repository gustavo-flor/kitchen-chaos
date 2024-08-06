using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        CameraForward,
        CameraForwardInverted
    }
    
    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        var mainCamera = Camera.main;
        if (!mainCamera) return;
        switch (mode)
        {
            case Mode.CameraForward:
                transform.forward = mainCamera.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -mainCamera.transform.forward;
                break;
        }
    }
}

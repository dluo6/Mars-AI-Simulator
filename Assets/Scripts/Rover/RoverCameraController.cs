using UnityEngine;
using Unity.Cinemachine;

public class RoverCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private RoverManager roverManager;

    private void Start()
    {
        if (roverManager == null)
            roverManager = FindFirstObjectByType<RoverManager>();

        roverManager.OnActiveRoverChanged += OnActiveRoverChanged;

        OnActiveRoverChanged(roverManager.ActiveRover);
    }

    private void OnActiveRoverChanged(GameObject newRover)
    {
        if (newRover != null && cinemachineCamera != null)
        {
            cinemachineCamera.Follow = newRover.transform;
            cinemachineCamera.LookAt = newRover.transform;
        }
    }

    private void OnDestroy()
    {
        if (roverManager != null)
            roverManager.OnActiveRoverChanged -= OnActiveRoverChanged;
    }
}
using UnityEngine;
using Unity.Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera CinCamera;
    [SerializeField] private CinemachineFollow CinFollow;

    private Vector3 followOffset = new Vector3(15f, 7f, -1f);
    [SerializeField] private float cameraFOV = 60f;

    private RoverManager currentRoverManager;

    private void Start()
    {
        InitializeCamera();
        GlobalVariables.Instance.OnPlayerChanged += OnPlayerChanged;
        OnPlayerChanged(GlobalVariables.Instance.currentPlayerIndex);
    }

    private void InitializeCamera()
    {
        if (CinFollow != null)
        {
            CinFollow.FollowOffset = followOffset;
        }
    }

    private void OnPlayerChanged(int playerIndex)
    {
        if (currentRoverManager != null)
        {
            currentRoverManager.OnActiveRoverChanged -= OnActiveRoverChanged;
        }

        GameObject currentPlayer = GlobalVariables.Instance.players[playerIndex];
        currentRoverManager = currentPlayer.GetComponent<RoverManager>();

        if (currentRoverManager != null)
        {
            currentRoverManager.OnActiveRoverChanged += OnActiveRoverChanged;
            UpdateCameraTarget(currentRoverManager.ActiveRover); // Immediate update
        }
    }

    private void OnActiveRoverChanged(GameObject newRover)
    {
        UpdateCameraTarget(newRover);
    }

    private void UpdateCameraTarget(GameObject targetRover)
    {
        if (targetRover != null && CinCamera != null)
        {
            CinCamera.Lens.FieldOfView = cameraFOV;
            CinCamera.Follow = targetRover.transform;
            CinCamera.LookAt = targetRover.transform;
        }
    }

    private void OnDestroy()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnPlayerChanged -= OnPlayerChanged;
        }

        if (currentRoverManager != null)
        {
            currentRoverManager.OnActiveRoverChanged -= OnActiveRoverChanged;
        }
    }
}
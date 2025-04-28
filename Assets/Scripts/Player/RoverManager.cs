using UnityEngine;

public class RoverManager : MonoBehaviour
{
    [SerializeField] private GameObject rover1Prefab;
    [SerializeField] private GameObject rover2Prefab;
    [SerializeField] private float roverSpacing = 3f;

    private GameObject[] rovers;
    private int currentRoverIndex = 0;
    private GameObject activeRover;
    private bool isCurrentPlayer = false;

    public GameObject ActiveRover => activeRover;
    public event System.Action<GameObject> OnActiveRoverChanged;

    private float flipThreshold = 80f;
    private float respawnHeight = 3f;
    private string playerName = "Rover";

    void Awake()
    {
        InitializeRovers();
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnPlayerChanged += HandlePlayerChange;
            HandlePlayerChange(GlobalVariables.Instance.currentPlayerIndex);
        }
    }

    void HandlePlayerChange(int newPlayerIndex)
    {
        if (GlobalVariables.Instance == null) return;
        int myIndex = GlobalVariables.Instance.players.IndexOf(gameObject);
        isCurrentPlayer = myIndex == newPlayerIndex;
    }

    void OnDestroy()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.OnPlayerChanged -= HandlePlayerChange;
        }
    }

    void InitializeRovers()
    {
        // Debug.Log("Initializing Rovers");
        rovers = new GameObject[2];

        // Spawn rovers as child objects
        rovers[0] = Instantiate(
            rover1Prefab,
            transform.position + Vector3.left * roverSpacing,
            Quaternion.identity,
            transform); // Parent to this player

        rovers[1] = Instantiate(
            rover2Prefab,
            transform.position + Vector3.right * roverSpacing,
            Quaternion.identity,
            transform); // Parent to this player

        SetActiveRover(0);
    }

    void Update()
    {
        if (!isCurrentPlayer) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchRover();
        }

        if (activeRover != null && Vector3.Angle(activeRover.transform.up, Vector3.up) > flipThreshold)
        {
            Respawn();
        }
    }

    void SetActiveRover(int index)
    {
        if (rovers == null || index < 0 || index >= rovers.Length) return;

        // Store current position/rotation
        Vector3 currentPosition = activeRover != null ? activeRover.transform.position : transform.position;
        Quaternion currentRotation = activeRover != null ? activeRover.transform.rotation : transform.rotation;

        // Deactivate all rovers
        foreach (var rover in rovers)
        {
            if (rover != null) rover.SetActive(false);
        }

        // Set new active rover
        currentRoverIndex = index;
        activeRover = rovers[index];

        if (activeRover != null)
        {
            // Update position/rotation
            activeRover.transform.position = currentPosition;
            activeRover.transform.rotation = currentRotation * Quaternion.Euler(0, 180, 0);
            activeRover.SetActive(true);

            // Reset physics
            if (activeRover.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            OnActiveRoverChanged?.Invoke(activeRover);
        }
    }

    void SwitchRover()
    {
        int nextIndex = (currentRoverIndex + 1) % rovers.Length;
        SetActiveRover(nextIndex);
    }

    private void Respawn()
    {
        if (activeRover == null) return;

        Vector3 newPos = activeRover.transform.position + Vector3.up * respawnHeight;
        activeRover.transform.position = newPos;
        activeRover.transform.rotation = Quaternion.Euler(0, activeRover.transform.eulerAngles.y, 0);

        if (activeRover.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public string GetPlayerName() => playerName;
    public void SetPlayerName(string newName) => playerName = newName;
}
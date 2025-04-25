using UnityEngine;

public class RoverManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject rover1Prefab;
    [SerializeField] private GameObject rover2Prefab;
    [SerializeField] private float roverSpacing = 3f;

    private GameObject[] rovers;
    private int currentRoverIndex = 0;
    private GameObject activeRover;

    public GameObject ActiveRover => activeRover;
    public event System.Action<GameObject> OnActiveRoverChanged;

    private float flipThreshold = 80f;
    private float respawnHeight = 3f;

    private string playerName;

    void Start()
    {
        InitializeRovers();
    }

    void InitializeRovers()
    {
        rovers = new GameObject[2];

        // Spawn rovers as independent objects (no parent)
        rovers[0] = Instantiate(rover1Prefab, transform.position + Vector3.left * roverSpacing, Quaternion.identity);
        rovers[1] = Instantiate(rover2Prefab, transform.position + Vector3.right * roverSpacing, Quaternion.identity);

        SetActiveRover(0);
    }

    void Update()
    {
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
        // Store current rover's position and rotation before switching
        Vector3 currentPosition = activeRover != null ? activeRover.transform.position : transform.position;
        Quaternion currentRotation = activeRover != null ? activeRover.transform.rotation : Quaternion.identity;

        // Deactivate all rovers
        foreach (var rover in rovers)
        {
            rover.SetActive(false);
        }

        // Set new active rover
        currentRoverIndex = index;
        activeRover = rovers[index];

        // Move new rover to previous rover's position
        activeRover.transform.position = currentPosition;
        activeRover.transform.rotation = index == 1 ? currentRotation * Quaternion.Euler(0, 180, 0) : currentRotation * Quaternion.Euler(0, 180, 0);

        activeRover.SetActive(true);

        // Reset physics if needed
        if (activeRover.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        OnActiveRoverChanged?.Invoke(activeRover);
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

    public string getPlayerName()
    {
        return playerName;
    }

    public void setPlayerName(string newName)
    {
        playerName = newName;
    }
}
using UnityEngine;

public class RoverManager : MonoBehaviour
{
    private string playerName;

    [Header("Prefabs")]
    [SerializeField] private GameObject rover1Prefab;
    [SerializeField] private GameObject rover2Prefab;

    [Header("Settings")]
    [SerializeField] private Transform roverContainer;
    [SerializeField] private float roverSpacing = 3f;

    private GameObject[] rovers;
    private int currentRoverIndex = 0;
    private GameObject activeRover;

    public GameObject ActiveRover => activeRover;
    public event System.Action<GameObject> OnActiveRoverChanged;

    private float flipThreshold = 80f; // Angle to consider upside-down
    private float respawnHeight = 3f;

    void Start()
    {
        InitializeRovers();
    }

    void InitializeRovers()
    {
        rovers = new GameObject[2];

        // Instantiate rovers at player's position
        rovers[0] = Instantiate(rover1Prefab, transform.position, Quaternion.identity, roverContainer);
        rovers[1] = Instantiate(rover2Prefab, transform.position, Quaternion.identity, roverContainer);

        // Apply local offsets
        rovers[0].transform.localPosition = Vector3.left * roverSpacing;
        rovers[1].transform.localPosition = Vector3.right * roverSpacing;

        SetActiveRover(0);
    }

    void SetActiveRover(int index)
    {
        // Store current player position (not rover position)
        Vector3 previousPosition = transform.position;

        foreach (var rover in rovers)
        {
            rover.SetActive(false);
        }

        currentRoverIndex = index;
        activeRover = rovers[index];
        activeRover.SetActive(true);

        // Position handling - KEY FIX
        activeRover.transform.position = previousPosition;
        activeRover.transform.localPosition = Vector3.zero; // Reset offset
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchRover();
        }

        if (activeRover != null &&
            Vector3.Angle(activeRover.transform.up, Vector3.up) > flipThreshold)
        {
            Respawn();
        }
    }
    void SwitchRover()
    {
        int nextIndex = (currentRoverIndex + 1) % rovers.Length;
        SetActiveRover(nextIndex);
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public void setPlayerName(string newName)
    {
        playerName = newName;
    }

    private void Respawn()
    {
        if (activeRover == null) return;

        // Move both player and rover together
        Vector3 newPos = activeRover.transform.position + Vector3.up * respawnHeight;
        transform.position = newPos;
        activeRover.transform.position = newPos;

        // Reset rotations
        Quaternion newRot = Quaternion.Euler(0, activeRover.transform.eulerAngles.y, 0);
        transform.rotation = newRot;
        activeRover.transform.rotation = newRot;

        // Only reset rover's physics
        if (activeRover.TryGetComponent<Rigidbody>(out var roverRb))
        {
            roverRb.linearVelocity = Vector3.zero;
            roverRb.angularVelocity = Vector3.zero;
        }
    }
}
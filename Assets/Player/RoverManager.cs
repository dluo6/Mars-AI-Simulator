using UnityEngine;

public class RoverManager : MonoBehaviour
{

    private string PlayerName;
    [SerializeField] private GameObject rover1Prefab;
    [SerializeField] private GameObject rover2Prefab;
    public KeyCode switchKey = KeyCode.Tab;
    public float spawnHeight = 5f; // Adjust this in Inspector to change drop height
    public GameObject[] Rovers;
    private int currentRoverIndex = 0;
    public event System.Action<GameObject> OnRoverChanged;
    public GameObject CurrentActiveRover => Rovers[currentRoverIndex];

    void Start()
    {

        // Instantiate both rovers from your prefabs
        Rovers = new GameObject[2];
        Rovers[0] = Instantiate(rover1Prefab, transform.position, Quaternion.identity);
        Rovers[1] = Instantiate(rover2Prefab, transform.position + Vector3.right * 3f, Quaternion.identity);
        SetActiveRover(currentRoverIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            SwitchRover();
        }
    }

    void SwitchRover()
    {
        // Store current position and rotation
        Vector3 currentPosition = CurrentActiveRover.transform.position;
        Quaternion currentRotation = CurrentActiveRover.transform.rotation;

        // Deactivate current rover
        CurrentActiveRover.SetActive(false);

        // Move to next rover
        currentRoverIndex = (currentRoverIndex + 1) % Rovers.Length;

        // Position and activate new rover
        Rovers[currentRoverIndex].transform.position = currentPosition + Vector3.up * spawnHeight;
        Rovers[currentRoverIndex].transform.rotation = currentRotation;
        Rovers[currentRoverIndex].SetActive(true);

        OnRoverChanged?.Invoke(CurrentActiveRover);
    }

    void SetActiveRover(int index)
    {
        for (int i = 0; i < Rovers.Length; i++)
        {
            Rovers[i].SetActive(i == index);
        }
    }

    public void SetPlayerName(string newName)
    {
        PlayerName = newName;
    }

    string GetPlayerName()
    {
        return PlayerName;
    }
}
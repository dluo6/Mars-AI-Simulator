using UnityEngine;

public class RoverManager : MonoBehaviour
{
    public GameObject[] Rovers;
    private int currentRoverIndex = 0;

    public KeyCode switchKey = KeyCode.Tab;
    public float spawnHeight = 5f; // Adjust this in Inspector to change drop height

    public event System.Action<GameObject> OnRoverChanged;
    public GameObject CurrentRover => Rovers[currentRoverIndex];

    void Start()
    {
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
        Vector3 currentPosition = Rovers[currentRoverIndex].transform.position;
        Quaternion currentRotation = Rovers[currentRoverIndex].transform.rotation;

        // Deactivate current Rover
        Rovers[currentRoverIndex].SetActive(false);

        // Move to next Rover
        currentRoverIndex = (currentRoverIndex + 1) % Rovers.Length;

        // Set new Rover's position (with height offset) and activate
        Rovers[currentRoverIndex].transform.position = currentPosition + Vector3.up * spawnHeight;
        Rovers[currentRoverIndex].transform.rotation = currentRotation;
        OnRoverChanged?.Invoke(Rovers[currentRoverIndex]);
        Rovers[currentRoverIndex].SetActive(true);
    }

    void SetActiveRover(int index)
    {
        for (int i = 0; i < Rovers.Length; i++)
        {
            Rovers[i].SetActive(i == index);
        }
    }
}
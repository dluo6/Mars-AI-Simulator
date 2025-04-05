using System.Collections.Generic;
using UnityEngine;

public class GameSimulation : MonoBehaviour
{
    public GameObject rover;
    private List<GameObject> rovers = new List<GameObject>();

    public void Start() {
        // Instantiate the rover
        foreach (Vector3 pos in GlobalVariables.Instance.startCoordinates) {
            rovers.Add(Instantiate(rover, pos, new Quaternion(0,0,0,0)));
        }
        // To speed up the simulation - can change later
        Time.timeScale = 1f;
    }
}
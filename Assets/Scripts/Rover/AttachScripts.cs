using UnityEngine;
using TMPro;

public class AttachScripts : MonoBehaviour
{
    public Terrain marsTerrain;
    void Start()
    {
        foreach (GameObject player in GlobalVariables.Instance.players)
        {
            GameObject activeRover = player.GetComponent<RoverManager>().ActiveRover;
            CheckWetArea wetAreaScript = activeRover.AddComponent<CheckWetArea>();
            wetAreaScript.setTerrain(marsTerrain);
        }
    }
}

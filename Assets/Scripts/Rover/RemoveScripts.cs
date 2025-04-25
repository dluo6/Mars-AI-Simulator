using UnityEngine;

public class RemoveScripts : MonoBehaviour
{

    public void Start()
    {
        foreach (GameObject player in GlobalVariables.Instance.players)
        {
            GameObject activeRover = player.GetComponent<RoverManager>().ActiveRover;
            DestroyImmediate(activeRover.GetComponent<CheckWetArea>());
        }
    }
}
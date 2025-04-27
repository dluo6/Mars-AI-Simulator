using UnityEngine;
using TMPro;

public class AttachScripts : MonoBehaviour
{
    public Terrain marsTerrain;
    void Start()
    {
        foreach (GameObject player in GlobalVariables.Instance.players)
        {
            player.AddComponent<StatsPanel>();
            CheckWetArea wetAreaScript = player.AddComponent<CheckWetArea>();
            wetAreaScript.setTerrain(marsTerrain);
        }
    }
}

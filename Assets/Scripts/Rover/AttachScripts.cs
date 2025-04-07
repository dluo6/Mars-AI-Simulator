using UnityEngine;
using TMPro;

public class AttachScripts : MonoBehaviour
{
    public Terrain marsTerrain;
    void Start()
    {
        foreach (GameObject rover in GlobalVariables.Instance.rovers) {
            CheckWetArea wetAreaScript = rover.AddComponent<CheckWetArea>();
            wetAreaScript.setTerrain(marsTerrain);
        }
    }
}

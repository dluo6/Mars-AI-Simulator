using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterConfigUI : MonoBehaviour
{
    public void SelectConfig(int configIndex)
    {
        Debug.Log($"Selected water config: {configIndex}");
        WaterConfigSelection.SelectedConfigIndex = configIndex;
        SceneManager.LoadScene("TerrainMars");
    }

}
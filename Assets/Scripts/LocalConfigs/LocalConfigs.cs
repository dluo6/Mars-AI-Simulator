using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalConfigs : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("TerrainMars");
    }
}

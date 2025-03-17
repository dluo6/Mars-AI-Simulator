using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    public void BackToGame()
    {
        // TODO: Replace with more appropriate scene
        SceneManager.LoadScene("TerrainMars");
    }

    public void ExitGame()
    {
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }
}

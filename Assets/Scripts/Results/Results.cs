using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    public void ViewLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void BackToGame()
    {
        // TODO: Fix so that simulation isn't restarted each time
        SceneManager.LoadScene("TerrainMars");
    }

    public void ExitGame()
    {
        // Debug.LogWarning("Exit button pressed!");
        Application.Quit();
    }
}

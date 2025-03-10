using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void BackToMenu()
    {
        // Load the Main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with your actual game scene name)
        SceneManager.LoadScene("GlobalConfigs");
    }

    public void ShowCredits()
    {
        // Load the credits scene (replace "CreditsScene" with your actual credits scene name)
        SceneManager.LoadScene("CreditsScene");
    }

    public void ExitGame()
    {
        Debug.Log("Quit button pressed!");
        // Quit the application
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Reference to the buttons
    public Button startButton;
    public Button creditsButton;
    public Button exitButton;

    void Start()
    {
        // Add listeners to the buttons
        startButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(ShowCredits);
        exitButton.onClick.AddListener(ExitGame);
        Screen.fullScreen = true;
    }

    void StartGame()
    {
        // Load the game scene (replace "GameScene" with your actual game scene name)
        SceneManager.LoadScene("GameScene");
    }

    void ShowCredits()
    {
        // Load the credits scene (replace "CreditsScene" with your actual credits scene name)
        SceneManager.LoadScene("CreditsScene");
    }

    void ExitGame()
    {
        Debug.Log("Quit button pressed!");
        // Quit the application
        Application.Quit();
    }
}

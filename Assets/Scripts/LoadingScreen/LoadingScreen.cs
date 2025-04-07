using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingBox;   // Reference to the loading screen panel
    public TextMeshProUGUI loadingText;      // Reference to the loading text
    public GameObject player;         // Reference to the player (if you want to disable the player)
    public MarsClimate marsClimate;
    private bool calculationFinished = false;


    void Start()
    {
        // Show loading panel and freeze the scene
        loadingBox.SetActive(true);
        FreezeScene(true);

        StartCoroutine(RunLoadingProcess());
    }

    private IEnumerator RunLoadingProcess()
    {
        // Start both coroutines, but wait for calculations to finish
        var loadingAnimation = StartCoroutine(LoadingAnimation());
        yield return StartCoroutine(PerformClimateCalculations());

        // Calculations are done, stop the loading animation
        StopCoroutine(loadingAnimation);
        loadingText.text = "Loading completed!";

        // After calculations are done, unfreeze the scene
        FreezeScene(false);

        // Hide the loading panel after work is complete
        loadingBox.SetActive(false);
    }


    private IEnumerator PerformClimateCalculations()
    {

        yield return StartCoroutine(marsClimate.ApplyClimate());
        calculationFinished = true;
        FreezeScene(false);
        loadingBox.SetActive(false);
    }

    private IEnumerator LoadingAnimation()
    {
        // Loop through loading text animation
        while (!calculationFinished)
        {
            loadingText.text = "Loading\nPlease wait";
            yield return new WaitForSecondsRealtime(0.25f);

            loadingText.text = "Loading.\nPlease wait.";
            yield return new WaitForSecondsRealtime(0.25f);  // Wait for half a second

            loadingText.text = "Loading..\nPlease wait..";
            yield return new WaitForSecondsRealtime(0.25f);

            loadingText.text = "Loading...\nPlease wait...";
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }


    private void FreezeScene(bool freeze)
    {
        if (freeze)
        {
            // Freeze the scene: Stop player movement, freeze physics, etc.
            Time.timeScale = 0f;  // Stop time (freeze all gameplay)
        }
        else
        {
            // Unfreeze the scene: Resume gameplay
            Time.timeScale = 1f;  // Resume time
        }
    }
}
using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingBox;   // Reference to the loading screen panel
    public TextMeshProUGUI loadingText;      // Reference to the loading text
    public GameObject player;         // Reference to the player (if you want to disable the player)
    public MarsClimate marsClimate;


    void Start()
    {
        // Show loading panel and freeze the scene
        loadingBox.SetActive(true);
        FreezeScene(true);

        StartCoroutine(LoadingAnimation());
        StartCoroutine(PerformClimateCalculations());
    }

    private IEnumerator PerformClimateCalculations()
    {

        bool appliedClimate = marsClimate.ApplyClimate();
        while (!appliedClimate)
        {
            yield return null;
        }

        // After calculations are done, unfreeze the scene
        FreezeScene(false);

        // Hide the loading panel after work is complete
        loadingBox.SetActive(false);
    }

    private IEnumerator LoadingAnimation()
    {
        // Loop through loading text animation
        while (true)
        {
            loadingText.text = "Loading.\nPlease wait...";
            yield return new WaitForSeconds(0.25f);  // Wait for half a second

            loadingText.text = "Loading..\nPlease wait...";
            yield return new WaitForSeconds(0.25f);

            loadingText.text = "Loading...\nPlease wait...";
            yield return new WaitForSeconds(0.25f);
        }
    }


    private void FreezeScene(bool freeze)
    {
        if (freeze)
        {
            // Freeze the scene: Stop player movement, freeze physics, etc.
            Time.timeScale = 0f;  // Stop time (freeze all gameplay)
            if (player != null)
            {
                // Optionally disable player movement (if you have player scripts)
                player.SetActive(false); // Disable player object (or just player movement script)
            }

            // You can disable other gameplay-related scripts if necessary
        }
        else
        {
            // Unfreeze the scene: Resume gameplay
            Time.timeScale = 1f;  // Resume time
            if (player != null)
            {
                // Re-enable player object
                player.SetActive(true); // Enable player object (or just player movement script)
            }
        }
    }
}

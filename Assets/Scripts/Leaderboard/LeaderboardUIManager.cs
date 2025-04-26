using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class LeaderboardUIManager : MonoBehaviour 
{
    // reference to UI elements
    public Transform resultsContainer;
    public GameObject resultItemPrefab;
    
    // reference to the DatabaseManager
    private DatabaseManager _databaseManager;
    
    void Start()
    {
        // find the DatabaseManager in the scene
        _databaseManager = FindObjectOfType<DatabaseManager>();
        
        if (_databaseManager == null)
        {
            Debug.LogError("DatabaseManager not found in the scene. Make sure it exists.");
            return;
        }
        
        // load and display the results
        DisplayLeaderboardData();
    }

    public void OnExitClicked() 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnProgressReportClicked() 
    {
        SceneManager.LoadScene("EndGameProgress");
    }
    
    private void DisplayLeaderboardData()
    {
        // get results from DatabaseManager
        List<RoverResult> results = _databaseManager.LoadResults();
        
        // sort results by water bodies (descending) and time elapsed (ascending)
        results.Sort((a, b) => {
            // first compare by water bodies (descending)
            int waterComparison = b.WaterBodies.CompareTo(a.WaterBodies);
            if (waterComparison != 0)
                return waterComparison;
                
            // If water bodies are equal, compare by time elapsed (ascending)
            return a.TimeElapsed.CompareTo(b.TimeElapsed);
        });
        
        // clear existing result items
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // create new result items
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            
            // instantiate the result item prefab
            GameObject resultItemObj = Instantiate(resultItemPrefab, resultsContainer);
            
            // set position in the list
            resultItemObj.transform.SetSiblingIndex(i);
            
            // find the text components
            var texts = resultItemObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 5) // Make sure we have enough text elements
            {
                // update the text values (adjust indices based on prefab layout)
                texts[0].text = (i + 1).ToString();  // rank
                texts[1].text = result.RoverName;
                texts[2].text = result.WaterBodies.ToString();
                texts[3].text = FormatTime(result.TimeElapsed);
                texts[4].text = result.TerrainDiscovered.ToString("F2") + "%";
            }
        }
    }
    
    private string FormatTime(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
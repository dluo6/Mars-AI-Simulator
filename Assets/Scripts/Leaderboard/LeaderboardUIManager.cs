using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using TMPro;

public class LeaderboardUIManager : MonoBehaviour 
{
    // reference to UI elements
    [SerializeField] private Transform resultsContainer;
    [SerializeField] private GameObject resultItemPrefab;
    [SerializeField] private int maxLeaderboardEntries = 5; // show only top 5 results
    
    void Start()
    {
        // check if DatabaseManager.Instance exists
        if (DatabaseManager.Instance == null)
        {
            Debug.LogError("DatabaseManager.Instance is null. Make sure DatabaseManager exists in the scene.");
            return;
        }
        
        // uncomment to add test entry, REMOVE IN PRODUCTION
        // AddTestEntry();

        // load and display the results
        DisplayLeaderboardData();
    }

    // method to test new leaderboard entry
    // public void AddTestEntry()
    // {
    //     // Create a new test rover result
    //     RoverResult testRover = new RoverResult
    //     {
    //         RoverName = "NewRover",
    //         WaterBodies = 100,
    //         TerrainDiscovered = 80f,
    //         TimeElapsed = 300, // 5 minutes
    //         UserID = 99 // Adjust as needed for your database schema
    //     };
        
    //     // Insert into database
    //     DatabaseManager.Instance.InsertResult(testRover);
        
    //     // Refresh the display to show the new entry
    //     DisplayLeaderboardData();
        
    //     Debug.Log("Added test entry: NewRover with 100 water bodies and 80% terrain discovered");
    // }

    // quits application on click
    public void OnExitClicked() 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // goes back to progress report page upon clicking button
    public void OnProgressReportClicked() 
    {
        SceneManager.LoadScene("EndGameProgress");
    }
    
    private void DisplayLeaderboardData()
    {
        // Check for null references
        if (resultsContainer == null)
        {
            Debug.LogError("Results Container is not assigned in the Inspector");
            return;
        }
        
        if (resultItemPrefab == null)
        {
            Debug.LogError("Result Item Prefab is not assigned in the Inspector");
            return;
        }
        
        // get results from DatabaseManager using the singleton Instance
        List<RoverResult> results = DatabaseManager.Instance.LoadResults();
        
        // sort results by WaterBodies descending & TerrainDiscovered descending
        results.Sort((a, b) => b.WaterBodies == a.WaterBodies ?
            b.TerrainDiscovered.CompareTo(a.TerrainDiscovered) :
            b.WaterBodies.CompareTo(a.WaterBodies));
        
        // clear existing result items
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // take only the top N results
        int entriesToShow = Mathf.Min(maxLeaderboardEntries, results.Count);
        
        // create new result items
        for (int i = 0; i < entriesToShow; i++)
        {
            var result = results[i];
            
            // instantiate the result item prefab
            GameObject resultItemObj = Instantiate(resultItemPrefab, resultsContainer);
            
            // set position in the list
            resultItemObj.transform.SetSiblingIndex(i);
            
            // use the ResultItemUI component
            ResultItemUI resultUI = resultItemObj.GetComponent<ResultItemUI>();
            if (resultUI != null)
            {
                resultUI.SetData(
                    rank: i + 1,
                    roverName: result.RoverName,
                    waterBodies: result.WaterBodies,
                    terrainDiscovered: result.TerrainDiscovered
                    // timeElapsed: result.TimeElapsed
                );
            }
            else
            {
                Debug.LogError("ResultItemUI component not found on prefab. Please add it to your result item prefab.");
            }
        }
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using TMPro;

public class LeaderboardResult
{
    public int ResultID;
    public int UserID;
    public string RoverName;
    public int WaterBodies;
    public int TimeElapsed;
    public float TerrainDiscovered;
}

public class LeaderboardUIManager : MonoBehaviour {

    public Transform resultsContainer;   // Parent object to hold result items
    public GameObject resultItemPrefab;  // Prefab for each result item

    private List<LeaderboardResult> leaderboardResults = new List<LeaderboardResult>();

    void Start()
    {
        // Load data when the scene starts
        LoadLeaderboardData();
        DisplayLeaderboardData();
    }

    public void OnExitClicked() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnProgressReportClicked() {
        SceneManager.LoadScene("EndGameProgress");
    }

    private void LoadLeaderboardData()
    {
        // Clear previous results
        // leaderboardResults.Clear();

        // // Get the project root directory
        // string projectRoot = Application.dataPath;  // This gives /Assets
        // projectRoot = Directory.GetParent(projectRoot).FullName;  // Go up one level to project root
        
        // Combine with the relative path to your database
        string dbPath = Path.Combine(projectRoot, "Database", "mars_ai_simulator.db");
        
        // this is to check if file exists
        // // Get the path to the database file
        // string dbPath = Path.Combine(Application.streamingAssetsPath, "mars_ai_simulator.db");
        
        // if (!File.Exists(dbPath))
        // {
        //     Debug.LogError("Database file not found at: " + dbPath);
        //     return;
        // }
        
        // Debug.Log("Using database at: " + dbPath);

        // Create a database connection
        string connectionString = "URI=file:" + dbPath;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            
            // Create and execute the query to get results
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ResultID, UserID, RoverName, WaterBodies, TimeElapsed, TerrainDiscovered FROM results ORDER BY WaterBodies DESC, TimeElapsed ASC";
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var result = new LeaderboardResult
                        {
                            ResultID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            RoverName = reader.GetString(2),
                            WaterBodies = reader.GetInt32(3),
                            TimeElapsed = reader.GetInt32(4),
                            TerrainDiscovered = reader.GetFloat(5)
                        };
                        
                        leaderboardResults.Add(result);
                    }
                }
            }
            
            connection.Close();
        }
    }

    private void DisplayLeaderboardData()
    {
        // // Clear existing result items
        // foreach (Transform child in resultsContainer)
        // {
        //     Destroy(child.gameObject);
        // }
        
        // Create new result items for each result
        for (int i = 0; i < leaderboardResults.Count; i++)
        {
            var result = leaderboardResults[i];
            
            // Instantiate the result item prefab
            GameObject resultItemObj = Instantiate(resultItemPrefab, resultsContainer);
            
            // Set position in the list
            resultItemObj.transform.SetSiblingIndex(i);
            
            // Find the text components
            var texts = resultItemObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 5) // Make sure we have enough text elements
            {
                // Update the text values (adjust indices based on your prefab layout)
                texts[0].text = (i + 1).ToString();  // Rank
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
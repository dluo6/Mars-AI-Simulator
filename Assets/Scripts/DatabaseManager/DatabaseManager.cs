using UnityEngine;
using SQLite4Unity3d;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private SQLiteConnection _connection;
    private string dbPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Construct the database path. Make sure the database file (mars_ai_simulator.db)
            // is in your Assets/StreamingAssets folder.
            dbPath = Path.Combine(Application.streamingAssetsPath, "mars_ai_simulator.db");
            Debug.Log("Database path: " + dbPath);

            // Open the connection with read/write and create flags.
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<RoverResult> LoadResults()
    {
        List<RoverResult> results = new List<RoverResult>();
        var query = _connection.Table<RoverResult>();
        foreach (var item in query)
        {
            results.Add(item);
        }
        return results;
    }


    // Inserts a new result into the 'results' table.
    public void InsertResult(RoverResult newResult)
    {
        _connection.Insert(newResult);
    }

    public void UpdateResult(RoverResult updatedResult)
    {
        _connection.Update(updatedResult);
    }
}

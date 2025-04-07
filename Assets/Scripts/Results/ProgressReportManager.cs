using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressReportManager : MonoBehaviour
{
    public static ProgressReportManager Instance;

    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject resultItemPrefab;
    [SerializeField] private int poolSize = 12;

    private List<GameObject> objectPool = new List<GameObject>();
    private List<RoverResult> results = new List<RoverResult>();
    private int activeItemCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (contentTransform == null || resultItemPrefab == null)
        {
            Debug.LogError("Missing references: Content Transform or Result Item Prefab");
            return;
        }

        SetupLayout();
        InitializePool();

        // Load results from the database.
        LoadAndDisplayResultsFromDB();
    }

    void SetupLayout()
    {
        var vlg = contentTransform.GetComponent<VerticalLayoutGroup>()
            ?? contentTransform.gameObject.AddComponent<VerticalLayoutGroup>();

        vlg.padding = new RectOffset(10, 10, 10, 10);
        vlg.spacing = 15;
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childForceExpandWidth = true;

        var csf = contentTransform.GetComponent<ContentSizeFitter>()
            ?? contentTransform.gameObject.AddComponent<ContentSizeFitter>();

        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    void InitializePool()
    {
        objectPool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(resultItemPrefab, contentTransform);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    GameObject GetPooledObject()
    {
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        var newObj = Instantiate(resultItemPrefab, contentTransform);
        objectPool.Add(newObj);
        return newObj;
    }

    public List<RoverResult> GetCurrentResults()
    {
        return results;
    }

    // Update progress for a given rover.
    public void UpdateRoverProgress(string roverName, int waterBodiesIncrement, float terrainDiscovered, int timeElapsed)
    {
        RoverResult result = results.Find(r => r.RoverName == roverName);
        if (result != null)
        {
            result.WaterBodies += waterBodiesIncrement;
            result.TerrainDiscovered = terrainDiscovered;
            result.TimeElapsed = timeElapsed;
            // (Optionally, update the record in the database here using an UPDATE query.)
        }
        else
        {
            result = new RoverResult
            {
                RoverName = roverName,
                WaterBodies = waterBodiesIncrement,
                TerrainDiscovered = terrainDiscovered,
                TimeElapsed = timeElapsed,
                UserID = 1,  // Set this as needed.
                // ResultID is auto-assigned.
            };
            results.Add(result);
            DatabaseManager.Instance.InsertResult(result);
        }
        DisplayResults();
    }

    // Load results from the database.
    public void LoadAndDisplayResultsFromDB()
    {
        results.Clear();
        results.AddRange(DatabaseManager.Instance.LoadResults());
        DisplayResults();
    }

    void DisplayResults()
    {
        // Deactivate previously active items.
        for (int i = 0; i < activeItemCount && i < objectPool.Count; i++)
            objectPool[i].SetActive(false);

        // Sort results (e.g., by WaterBodies descending, then TerrainDiscovered).
        results.Sort((a, b) => b.WaterBodies == a.WaterBodies ?
            b.TerrainDiscovered.CompareTo(a.TerrainDiscovered) :
            b.WaterBodies.CompareTo(a.WaterBodies));

        activeItemCount = results.Count;

        for (int i = 0; i < results.Count; i++)
        {
            var resultObj = GetPooledObject();
            var resultUI = resultObj.GetComponent<ResultItemUI>();
            if (resultUI != null)
            {
                resultUI.SetData(
                    rank: i + 1,
                    roverName: results[i].RoverName,
                    waterBodies: results[i].WaterBodies,
                    terrainDiscovered: results[i].TerrainDiscovered
                );
            }
        }
    }
}

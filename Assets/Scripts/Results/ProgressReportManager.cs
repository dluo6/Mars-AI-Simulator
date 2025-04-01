using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ProgressReportManager : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject resultItemPrefab;
    [SerializeField] private string apiUrl = "https://get-api-url/results/list";
    [SerializeField] private bool useDummyData = true;
    [SerializeField] private int poolSize = 12;

    List<GameObject> objectPool = new List<GameObject>();
    List<RoverResult> results = new List<RoverResult>();
    int activeItemCount = 0;

    [System.Serializable]
    public class RoverResult
    {
        public int ResultID;
        public int UserID;
        public string RoverName;
        public int WaterBodies;
        public int TimeElapsed;
        public float TerrainDiscovered;
    }

    [System.Serializable]
    class ResultsResponse { public RoverResult[] result; }

    void Start()
    {
        if (!contentTransform || !resultItemPrefab)
        {
            Debug.LogError("Missing references: Content Transform or Result Item Prefab");
            return;
        }

        SetupLayout();
        InitializePool();

        if (useDummyData) LoadAndDisplayDummyData();

        // TODO: Fix when remote API has been defined
        //else StartCoroutine(FetchAndDisplayResults());
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
            if (!obj.activeInHierarchy) 
            { 
                obj.SetActive(true); 
                return obj; 
            }

        var newObj = Instantiate(resultItemPrefab, contentTransform);
        objectPool.Add(newObj);
        return newObj;
    }

    void LoadAndDisplayDummyData()
    {
        results.Clear();

        for (int i = 1; i <= 9; i++)
            results.Add(new RoverResult
            {
                RoverName = "Rover " + i,
                WaterBodies = Random.Range(0, 5),
                TerrainDiscovered = Random.Range(5, 50)
            });

        DisplayResults();
    }

    IEnumerator FetchAndDisplayResults()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ResultsResponse response = JsonUtility.FromJson<ResultsResponse>(request.downloadHandler.text);
                if (response?.result != null)
                {
                    results.Clear();
                    results.AddRange(response.result);
                    DisplayResults();
                    yield break;
                }
            }

            Debug.Log("API error, using dummy data");
            LoadAndDisplayDummyData();
        }
    }

    void DisplayResults()
    {
        // Deactivate previously active items
        for (int i = 0; i < activeItemCount && i < objectPool.Count; i++)
            objectPool[i].SetActive(false);

        // Sort by WaterBodies then TerrainDiscovered (descending)
        results.Sort((a, b) => b.WaterBodies == a.WaterBodies
            ? b.TerrainDiscovered.CompareTo(a.TerrainDiscovered)
            : b.WaterBodies.CompareTo(a.WaterBodies));

        // Display results
        activeItemCount = results.Count;
        for (int i = 0; i < results.Count; i++)
        {
            var resultObj = GetPooledObject();
            var resultUI = resultObj.GetComponent<ResultItemUI>();
            if (resultUI != null)
            {
                resultUI.SetData(
                    i + 1, results[i].RoverName,
                    results[i].WaterBodies,
                    results[i].TerrainDiscovered
                );
            }
        }
    }
}

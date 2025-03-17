using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiItemTester : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject resultItemPrefab;

    [Header("Test Data")]
    [SerializeField] private int numberOfItems = 5;
    [SerializeField] private bool randomizeValues = true;
    [SerializeField] private bool testOnStart = true;

    // Sample test data sets
    private List<TestDataSet> testDataSets = new List<TestDataSet>();

    [System.Serializable]
    public class TestDataSet
    {
        public string roverName = "Test Rover";
        public int waterBodies = 0;
        public float terrainDiscovered = 10.0f;
    }

    void Start()
    {
        if (testOnStart)
        {
            SetupTestData();
            CreateTestItems();
        }
    }

    private void SetupTestData()
    {
        testDataSets.Clear();

        // Add some fixed test cases
        testDataSets.Add(new TestDataSet { roverName = "Rover 3", waterBodies = 1, terrainDiscovered = 14.0f });
        testDataSets.Add(new TestDataSet { roverName = "Rover 1", waterBodies = 0, terrainDiscovered = 12.0f });
        testDataSets.Add(new TestDataSet { roverName = "Rover 2", waterBodies = 0, terrainDiscovered = 9.0f });

        // Add random test cases if needed
        if (randomizeValues)
        {
            for (int i = 4; i <= numberOfItems; i++)
            {
                testDataSets.Add(new TestDataSet
                {
                    roverName = "Rover " + i,
                    waterBodies = Random.Range(0, 4),
                    terrainDiscovered = Random.Range(5, 75)
                });
            }
        }
    }

    public void CreateTestItems()
    {
        // Clear existing items
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Create new test items
        for (int i = 0; i < testDataSets.Count; i++)
        {
            GameObject resultItem = Instantiate(resultItemPrefab, contentParent);
            ResultItemUI itemUI = resultItem.GetComponent<ResultItemUI>();

            if (itemUI != null)
            {
                itemUI.SetData(i + 1,
                               testDataSets[i].roverName,
                               testDataSets[i].waterBodies,
                               testDataSets[i].terrainDiscovered);
            }
        }
    }

    // For inspector button
    public void RegenerateItems()
    {
        SetupTestData();
        CreateTestItems();
    }
}


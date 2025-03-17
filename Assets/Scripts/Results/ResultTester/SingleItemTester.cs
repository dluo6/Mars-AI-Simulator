using UnityEngine;

public class TestResultItem : MonoBehaviour
{
    private ResultItemUI resultItem;

    [Header("Test Data")]
    [SerializeField] private int rank = 1;
    [SerializeField] private string roverName = "Test Rover";
    [SerializeField] private int waterBodies = 3;
    [SerializeField] private float terrainDiscovered = 42.5f;

    [Header("Test Controls")]
    [SerializeField] private bool testOnStart = true;
    [SerializeField] private bool testOnButtonPress = false;

    void Start()
    {
        resultItem = GetComponent<ResultItemUI>();
        if (testOnStart && resultItem != null)
        {
            TestSetData();
        }
    }

    void Update()
    {
        if (testOnButtonPress && Input.GetKeyDown(KeyCode.Space) && resultItem != null)
        {
            TestSetData();
        }
    }

    public void TestSetData()
    {
        if (resultItem != null)
        {
            resultItem.SetData(rank, roverName, waterBodies, terrainDiscovered);
            Debug.Log($"Testing SetData with: Rank {rank}, Name {roverName}, Water {waterBodies}, Terrain {terrainDiscovered}%");
        }
        else
        {
            Debug.LogError("ResultItemUI component not found!");
        }
    }
}

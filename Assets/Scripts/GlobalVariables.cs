using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance;

    public int timeLimit = 1;
    public int numRovers = 1;
    public float simulationTimeElapsed = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // to enforce singleton behaviour
            Destroy(gameObject);
        }
    }
}
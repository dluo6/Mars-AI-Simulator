using UnityEngine;
using System.Collections.Generic;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance;

    public int timeLimit = 1;
    public int numRovers = 1;
    public float simulationTimeElapsed = 0;
    
    public List<GameObject> rovers = new List<GameObject>();

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

    public void AddToList(GameObject obj)
    {
        rovers.Add(obj);
        DontDestroyOnLoad(obj);
    }

    public void RemoveFromList(GameObject obj)
    {
        if (rovers.Contains(obj))
        {
            rovers.Remove(obj);
        }
    }
}
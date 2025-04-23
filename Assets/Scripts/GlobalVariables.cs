using UnityEngine;
using System.Collections.Generic;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance;

    public int timeLimit = 1;
    public int numPlayers = 1;
    public float simulationTimeElapsed = 0;

    public List<GameObject> players = new List<GameObject>();
    public int currentPlayerIndex = 0;
    public GameObject dummyPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GameObject instantiatedDummyPlayer = Instantiate(dummyPlayer, new Vector3(23499, 250, 14407), new Quaternion(0, 0, 0, 0));
            Instance.AddToList(instantiatedDummyPlayer);
        }
        else
        {
            // to enforce singleton behaviour
            Destroy(gameObject);
        }
    }

    public void AddToList(GameObject obj)
    {
        players.Add(obj);
        DontDestroyOnLoad(obj);
    }

    public void RemoveFromList(GameObject obj)
    {
        if (players.Contains(obj))
        {
            players.Remove(obj);
            Destroy(obj);
        }
    }
}
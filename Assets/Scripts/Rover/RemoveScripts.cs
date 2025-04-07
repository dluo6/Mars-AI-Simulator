using UnityEngine;

public class RemoveScripts: MonoBehaviour {

    public void Start()
    {
        foreach (GameObject rover in GlobalVariables.Instance.rovers) {
            DestroyImmediate(rover.GetComponent<CheckWetArea>());
        }   
    }
}
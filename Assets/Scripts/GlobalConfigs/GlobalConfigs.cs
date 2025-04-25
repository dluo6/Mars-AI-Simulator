using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalConfigs : MonoBehaviour
{
    public Slider timeLimit;
    public Slider numRovers;

    public void Proceed()
    {
        GlobalVariables.Instance.timeLimit = (int)timeLimit.value;
        GlobalVariables.Instance.numRovers = (int)numRovers.value;
        // Remove the dummy rover
        GlobalVariables.Instance.RemoveFromList(GlobalVariables.Instance.rovers[0]);
        SceneManager.LoadScene("LocalConfigs");
    }
}

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
        SceneManager.LoadScene("LocalConfigs");
    }
}

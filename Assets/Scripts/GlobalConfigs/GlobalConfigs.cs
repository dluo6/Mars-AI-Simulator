using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalConfigs : MonoBehaviour
{
    public Slider timeLimit;
    public Slider numPlayers;

    public void Proceed()
    {
        GlobalVariables.Instance.timeLimit = (int)timeLimit.value;
        GlobalVariables.Instance.numPlayers = (int)numPlayers.value;
        // Remove the dummy rover
        GlobalVariables.Instance.RemoveFromList(GlobalVariables.Instance.players[0]);
        SceneManager.LoadScene("LocalConfigs");
    }
}

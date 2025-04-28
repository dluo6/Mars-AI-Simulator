using UnityEngine;
using TMPro;

public class CameraSwitch : MonoBehaviour {

    public TextMeshProUGUI currentRoverName;

    void Start()
    {
        setName(GlobalVariables.Instance.currentPlayerIndex);
    } 

    public void LeftClicked() {
        int index = (GlobalVariables.Instance.currentPlayerIndex + GlobalVariables.Instance.players.Count - 1) % GlobalVariables.Instance.players.Count;
        setName(index);
        GlobalVariables.Instance.SwitchToPlayer(index);
    }

    public void RightClicked() {
        int index = (GlobalVariables.Instance.currentPlayerIndex + 1) % GlobalVariables.Instance.players.Count;
        setName(index);
        GlobalVariables.Instance.SwitchToPlayer(index);
    }

    void setName(int index) {
        RoverManager roverManager = GlobalVariables.Instance.players[index].GetComponent<RoverManager>();
        currentRoverName.text = roverManager.GetPlayerName();
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardUIManager : MonoBehaviour {
    public void OnExitClicked() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnProgressReportClicked() {
        SceneManager.LoadScene("EndGameProgress");
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeElapsed : MonoBehaviour
{

    public TextMeshProUGUI timeElapsedText;
    private float timeScale = 2.4f / 60f; // 1 real minute = 2.4 game hours


    // Update is called once per frame
    void Update()
    {
        // Simulate the game time: For every real second, 2.4/60 hours pass in the game
        GlobalVariables.Instance.simulationTimeElapsed += Time.deltaTime * timeScale;
        // Debug.Log("Simulated Game Time: " + GlobalVariables.Instance.simulationTimeElapsed * 24f + " hours");
        timeElapsedText.text = "Time Elapsed\n" + ((int) GlobalVariables.Instance.simulationTimeElapsed).ToString() + " Days";
        if (GlobalVariables.Instance.simulationTimeElapsed >= GlobalVariables.Instance.timeLimit)
        {
            // Perform any necessary actions when 1 day of simulation is completed
            Debug.Log("Simulation completed!");
            SceneManager.LoadScene("EndGameProgress");
        }
    }
}

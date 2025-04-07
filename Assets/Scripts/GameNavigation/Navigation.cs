using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{

    public GameObject escapePanel;

    private void Start()
    {
        escapePanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePanel.SetActive(true);
        }
    }

    public void ViewEndGameResults()
    {
        SceneManager.LoadScene("EndGameProgress");
    }


    public void CancelEscape()
    {
        escapePanel.SetActive(false);
    }


    public void ViewResults()
    {
        // Freeze the game while viewing results
        Time.timeScale = 0f;
        SceneManager.LoadScene("MidGameProgress");
    }
}
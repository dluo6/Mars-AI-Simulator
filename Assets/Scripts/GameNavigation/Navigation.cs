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
        SceneManager.LoadScene("MidGameProgress");
    }
}
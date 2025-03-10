using UnityEngine;
using UnityEngine.UI;

public class OpenURL : MonoBehaviour
{
    public string url;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenWebsite);
    }

    public void OpenWebsite()
    {
        if (!string.IsNullOrEmpty(url)) Application.OpenURL(url);
        else Debug.LogWarning("URL is empty or null");
    }
}
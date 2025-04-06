using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timeTextDisplay;

    private void Start()
    {
        if (timeTextDisplay == null)
        {
            timeTextDisplay = GetComponent<TMP_Text>();
        }

        UpdateDaysText();
    }

    public void UpdateDaysText()
    {
        if (GlobalVariables.Instance == null)
        {
            Debug.LogWarning("GlobalVariables Instance is null! Make sure it exists in the scene.");
            //return;
        }

        int days = (GlobalVariables.Instance == null) ? 1 : GlobalVariables.Instance.timeLimit;
        string dayText = days == 1 ? "DAY" : "DAYS";
        timeTextDisplay.text = $"TIME ELAPSED: {days} {dayText}";
    }
}

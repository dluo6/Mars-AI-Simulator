using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChangeSkin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private RectTransform skinSelectionPanel;
    [SerializeField] private RectTransform changeButtonRect;
    [SerializeField] private float animationSpeed = 1f; // Speed of the sliding animation

    private bool isChangingMode = false, initialised = false;
    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;

    void Start()
    {
        if (skinSelectionPanel == null || changeButtonRect == null)
        {
            skinSelectionPanel = GameObject.Find("Skin Panel").GetComponent<RectTransform>();
            changeButtonRect = GameObject.Find("Change Button").GetComponent<RectTransform>();
            Debug.LogWarning("Had to find Skin Panel and Change Button manually");
        }

        // Store the visible position (where the panel should be when shown)
        visiblePosition = changeButtonRect.anchoredPosition;
        visiblePosition.y += 110f;

        // Calculate the hidden position (off-screen above the button)
        hiddenPosition = changeButtonRect.anchoredPosition;
        hiddenPosition.y -= 200f;

        // Start with panel hidden
        skinSelectionPanel.anchoredPosition = hiddenPosition;
        initialised = true;
    }

    private void Update()
    {
        if (!isChangingMode && !initialised)
        {
            skinSelectionPanel.gameObject.SetActive(false);
        }
    }

    public void UpdateText()
    {
        // Toggle the state
        isChangingMode = !isChangingMode;

        // Change text based on state
        if (isChangingMode)
        {

            buttonText.text = "Exit Change Skin";
            skinSelectionPanel.gameObject.SetActive(true);
            StartCoroutine(AnimatePanel(hiddenPosition, visiblePosition));
        }
        else
        {
            buttonText.text = "Change Skin";
            skinSelectionPanel.gameObject.SetActive(false);
            StartCoroutine(AnimatePanel(visiblePosition, hiddenPosition));
        }
    }

    private IEnumerator AnimatePanel(Vector2 startPos, Vector2 endPos)
    {

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * animationSpeed;
            skinSelectionPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, time);
            yield return null;
        }

        // Ensure it ends exactly at the target position
        skinSelectionPanel.anchoredPosition = endPos;
    }
}
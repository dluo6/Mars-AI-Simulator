using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private string skinInfo;


    void Start()
    {
        if (infoPanel == null)
        {
            infoPanel = GameObject.Find("Skin Information Panel");
            infoText = GameObject.Find("Skin Information").GetComponent<TextMeshProUGUI>();
            Debug.LogWarning("Had to find Skin Info Panel manually");
        }
        infoPanel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoText.text = skinInfo;
            infoPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }
}

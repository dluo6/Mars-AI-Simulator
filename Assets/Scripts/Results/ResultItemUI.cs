using UnityEngine;
using TMPro;

public class ResultItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI roverNameText;
    [SerializeField] private TextMeshProUGUI waterBodiesText;
    [SerializeField] private TextMeshProUGUI terrainDiscoveredText;

    public void SetData(int rank, string roverName, int waterBodies, float terrainDiscovered)
    {
        rankText.text = rank.ToString() + ".";
        roverNameText.text = roverName;
        waterBodiesText.text = waterBodies.ToString();
        terrainDiscoveredText.text = terrainDiscovered.ToString("F0") + "%";
    }
}

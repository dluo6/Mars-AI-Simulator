using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    private Terrain marsTerrain;
    private MarsClimate marsClimate;
    private TextMeshProUGUI speedText;
    private TextMeshProUGUI temperatureText;
    private TextMeshProUGUI humidityText;
    private TextMeshProUGUI soilMoistureText;

    void Start()
    {
        marsTerrain = FindFirstObjectByType<Terrain>();
        marsClimate = FindFirstObjectByType<MarsClimate>();

        speedText = transform.Find("../Canvas/StatsPanel/StatsText/SpeedText")?.GetComponent<TextMeshProUGUI>();
        temperatureText = transform.Find("../Canvas/StatsPanel/StatsText/TemperatureText")?.GetComponent<TextMeshProUGUI>();
        humidityText = transform.Find("../Canvas/StatsPanel/StatsText/HumidityText")?.GetComponent<TextMeshProUGUI>();
        soilMoistureText = transform.Find("../Canvas/StatsPanel/StatsText/SoilMoistureText")?.GetComponent<TextMeshProUGUI>();

        if (marsTerrain == null || marsClimate == null ||
            speedText == null || temperatureText == null || humidityText || soilMoistureText)
        {
            Debug.LogError("Missing critical references!", this);
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject ActiveRover = GlobalVariables.Instance.players[GlobalVariables.Instance.currentPlayerIndex].GetComponent<RoverManager>().ActiveRover;
        IRoverController roverController = ActiveRover.GetComponent<IRoverController>();
        Vector3 roverPosition = ActiveRover.transform.position;
        speedText.text = "SPEED: " + ((int)roverController.GetCurrentSpeed()).ToString() + " m/s";
        temperatureText.text = "Temperature: " + marsClimate.GetTemperatureAtPosition(roverPosition).ToString("F2") + " C";
        humidityText.text = "Humidity: " + (marsClimate.GetHumidityAtPosition(roverPosition) * 100f).ToString("F2") + "%";
        soilMoistureText.text = "Soil Moisture: " + (marsClimate.GetSoilMoistureAtPosition(roverPosition) * 100f).ToString("F2") + "%";
    }
}

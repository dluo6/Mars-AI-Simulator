using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPanel : MonoBehaviour
{

    public Terrain marsTerrain;
    public MarsClimate marsClimate;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI humidityText;
    public TextMeshProUGUI soilMoistureText;
    public Vector3 currentRoverPosition;


    // Update is called once per frame
    void Update()
    {
        GameObject currentRover = GlobalVariables.Instance.players[GlobalVariables.Instance.currentPlayerIndex].GetComponent<RoverManager>().ActiveRover;
        // GameObject currentRover = GetComponent<RoverManager>().ActiveRover;
        IRoverController currentRoverController = currentRover.GetComponent<IRoverController>();
        currentRoverPosition = currentRover.transform.position;
        speedText.text = "SPEED: " + ((int)currentRoverController.GetCurrentSpeed()).ToString() + " m/s";
        temperatureText.text = "Temperature: " + marsClimate.GetTemperatureAtPosition(currentRoverPosition).ToString("F2") + " C";
        humidityText.text = "Humidity: " + (marsClimate.GetHumidityAtPosition(currentRoverPosition) * 100f).ToString("F2") + "%";
        soilMoistureText.text = "Soil Moisture: " + (marsClimate.GetSoilMoistureAtPosition(currentRoverPosition) * 100f).ToString("F2") + "%";
    }
}

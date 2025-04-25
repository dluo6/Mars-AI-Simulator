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
    public Vector3 roverPosition;


    // Update is called once per frame
    void Update()
    {
        GameObject rover = GlobalVariables.Instance.players[GlobalVariables.Instance.currentPlayerIndex].GetComponent<RoverManager>().ActiveRover;
        IRoverController roverController = rover.GetComponent<IRoverController>();
        roverPosition = rover.transform.position;
        speedText.text = "SPEED: " + ((int)roverController.GetCurrentSpeed()).ToString() + " m/s";
        temperatureText.text = "Temperature: " + marsClimate.GetTemperatureAtPosition(roverPosition).ToString("F2") + " C";
        humidityText.text = "Humidity: " + (marsClimate.GetHumidityAtPosition(roverPosition) * 100f).ToString("F2") + "%";
        soilMoistureText.text = "Soil Moisture: " + (marsClimate.GetSoilMoistureAtPosition(roverPosition) * 100f).ToString("F2") + "%";
    }
}

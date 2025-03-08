using UnityEngine;
using System.Collections.Generic;

public class MetaDataHoverDebug : MonoBehaviour
{

    public Terrain marsTerrain;
    public Camera mainCamera;
    public MarsClimate marsClimate;


    void Start()
    {
        //testAugmentClimateConditions();
    }


    void testAugmentClimateConditions()
    {
        // wherever the mouse is located at right now, it will augment climate for that region
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == marsTerrain.GetComponent<Collider>())
            {
                marsClimate.AugmentClimateConditions(new List<Vector3>() { hit.point });
            }
        }
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == marsTerrain.GetComponent<Collider>())
                {
                    float temperature = marsClimate.GetTemperatureAtPosition(hit.point);
                    float humidity = marsClimate.GetHumidityAtPosition(hit.point);
                    float soilMoisture = marsClimate.GetSoilMoistureAtPosition(hit.point);
                    Debug.Log("Temperature at this position: " + temperature);
                    Debug.Log("Humidity at this position: " + humidity);
                    Debug.Log("Soil Moisture at this position: " + soilMoisture);
                }
            }
        }
    }
}

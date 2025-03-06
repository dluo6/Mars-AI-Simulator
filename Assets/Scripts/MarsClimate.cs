using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class MarsClimate : MonoBehaviour
{
    public Terrain marsTerrain;
    public float maxTemp = 20f;
    public float minTemp = -100f;
    public float maxHumidity = 0.3f;
    public float minHumidity = 0f;
    public float maxSoilMoisture = 0.3f;
    public float minSoilMoisture = 0f;
    private float altFactor = 0.2f; // altitude factor to simulate temp drop with latitude

    private float[,] tempMap;
    private float[,] humidityMap;
    private float[,] soilMoistureMap;


    private void Start()
    {
        Debug.Log("Mars Climate Initiating!!!");

        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;
        tempMap = new float[heightMapResolution, heightMapResolution];
        humidityMap = new float[heightMapResolution, heightMapResolution];
        soilMoistureMap = new float[heightMapResolution, heightMapResolution];
        
        ApplyClimate();
    }


    // Update is called once per frame
    private void ApplyClimate()
    {
        int heightMapResolution = marsTerrain.terrainData.heightmapResolution; 
        float[,] heights = marsTerrain.terrainData.GetHeights(0, 0, heightMapResolution, heightMapResolution);

        float yConversionUnit = marsTerrain.terrainData.size.y;
        for (int x = 0; x < heightMapResolution; x++)
        {
            for (int y = 0; y < heightMapResolution; y++)
            {
                float altitude = heights[x, y] * yConversionUnit; // convert to meters
                float temp = Mathf.Lerp(maxTemp, minTemp, (altitude*altFactor)/ yConversionUnit); // interpolated value for this particular altitude   

                // Add variability based on latitude
                float latitudeFactor = Mathf.Abs(y / (float)heightMapResolution - 0.5f); // Latitude-based effect
                temp -= latitudeFactor * 50f; // Example adjustment based on latitude

                // Calculate humidity (higher at lower altitudes, lower at higher altitudes)
                float humidity = Mathf.Lerp(minHumidity, maxHumidity, ((yConversionUnit - (altitude * altFactor)) / yConversionUnit) * 0.05f);

                // Calculate soil moisture based on a combination of temperature, humidity, and altitude
                float soilMoisture = Mathf.Lerp(minSoilMoisture, maxSoilMoisture, (humidity * (1 - (altitude / yConversionUnit))) * (1 - Mathf.Abs(latitudeFactor)));
                soilMoisture += Random.Range(-0.0005f, 0.0005f); // some randomness to introduce realism
                soilMoisture = Mathf.Clamp(soilMoisture, minSoilMoisture, maxSoilMoisture);

                tempMap[x, y] = temp;
                humidityMap[x, y] = humidity;
                soilMoistureMap[x, y] = soilMoisture;
            }
        }
        Debug.Log("Done applying climate!!");

        // Below function is an expensive operation, i would suggest not to run it unless absolutely necessary to debug values at large
        //SaveMapsToCSV();
    }


    private int[] GetRelativeTerrainPosition(Vector3 absPosition)
    {
        // Convert the rover's absolute position into relative position inside the terrain
        Vector3 terrainPos = marsTerrain.transform.InverseTransformPoint(absPosition);

        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;

        // Calculate the relative position within the terrain
        int x = Mathf.FloorToInt((terrainPos.x * heightMapResolution) / marsTerrain.terrainData.size.x);
        int y = Mathf.FloorToInt((terrainPos.z * heightMapResolution) / marsTerrain.terrainData.size.z);

        // Clamp the values to ensure they are within the valid range
        x = Mathf.Clamp(x, 0, heightMapResolution - 1);
        y = Mathf.Clamp(y, 0, heightMapResolution - 1);
        return new int[] { x, y };
    }


    public float GetTemperatureAtPosition(Vector3 absPosition)
    {
        int[] relativePosition = GetRelativeTerrainPosition(absPosition);
        return tempMap[relativePosition[0], relativePosition[1]];
    }


    public float GetHumidityAtPosition(Vector3 absPosition)
    {
        int[] relativePosition = GetRelativeTerrainPosition(absPosition);
        return humidityMap[relativePosition[0], relativePosition[1]];
    }


    public float GetSoilMoistureAtPosition(Vector3 absPosition)
    {
        int[] relativePosition = GetRelativeTerrainPosition(absPosition);
        return soilMoistureMap[relativePosition[0], relativePosition[1]];
    }


    public void AugmentClimateConditions(List<Vector3> absCoordinates)
    {
        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;
        int radiusInPixels = Mathf.RoundToInt(1000 * (heightMapResolution / marsTerrain.terrainData.size.x)); // 1 km radius


        foreach (Vector3 coord in absCoordinates)
        {
            Debug.Log("Augmenting climate conditions as per secret water bodies....");

            int[] relativeCoord = GetRelativeTerrainPosition(coord);
            int centerX = relativeCoord[0];
            int centerY = relativeCoord[1];

            if (centerX < 0 || centerX >= heightMapResolution || centerY < 0 || centerY >= heightMapResolution)
            {
                Debug.LogWarning($"Coordinate ({centerX}, {centerY}) is outside terrain bounds, skipping.");
                continue;
            }

            for (int x = Mathf.Max(0, centerX - radiusInPixels); x < Mathf.Min(heightMapResolution, centerX + radiusInPixels); x++)
            {
                for (int y = Mathf.Max(0, centerY - radiusInPixels); y < Mathf.Min(heightMapResolution, centerY + radiusInPixels); y++)
                {
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));

                    if (distance <= radiusInPixels)
                    {
                        // Gaussian-like influence for smooth transitions
                        float influenceFactor = Mathf.Exp(-0.5f * (distance / radiusInPixels) * (distance / radiusInPixels));

                        //Water bodies moderate temperature, reducing extremes
                        float averageTemp = (maxTemp + minTemp) / 2f; // Average temperature
                        float tempAdjustment = influenceFactor * (averageTemp - tempMap[x, y]) * 0.1f; // Scale the adjustment

                        tempMap[x, y] += tempAdjustment;
                        humidityMap[x,y] += influenceFactor * (maxHumidity - humidityMap[x, y]);
                        soilMoistureMap[x, y] += influenceFactor * 0.15f;

                        // Clamping values to avoid exceeding limits
                        tempMap[x, y] = Mathf.Clamp(tempMap[x, y], minTemp, maxTemp);
                        humidityMap[x, y] = Mathf.Clamp(humidityMap[x, y], minHumidity, maxHumidity);
                        soilMoistureMap[x, y] = Mathf.Clamp(soilMoistureMap[x, y], minSoilMoisture, maxSoilMoisture);
                    }
                }
            }
        }

        Debug.Log("Augmented climate conditions successfully");
    }


    public void SaveMapsToCSV()
    {
        StringBuilder sbTemp = new StringBuilder();
        StringBuilder sbHumidity = new StringBuilder();
        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;
        for (int y = 0; y < heightMapResolution; y++)
        {
            for (int x = 0; x < heightMapResolution; x++)
            {
                sbTemp.Append(tempMap[x, y].ToString());
                sbHumidity.Append(humidityMap[x, y].ToString());
                if (x < heightMapResolution - 1)
                    sbTemp.Append(",");
                sbHumidity.Append(",");
            }
            sbTemp.AppendLine();
            sbHumidity.AppendLine();
        }

        File.WriteAllText("./temperature_map.csv", sbTemp.ToString());
        File.WriteAllText("./humidity_map.csv", sbHumidity.ToString());

        Debug.Log("climate data loaded into csv files!!");
    }   

}

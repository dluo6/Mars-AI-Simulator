using UnityEngine;
using System.IO;
using System.Text;

public class MarsClimate : MonoBehaviour
{
    public Terrain marsTerrain;
    public float maxTemp = 20f;
    public float minTemp = -100f;
    public float maxHumidity = 0.0003f;
    public float minHumidity = 0f;
    public float altFactor = 0.2f; // altitude factor to simulate temp drop with latitude

    private float[,] tempMap;
    private float[,] humidityMap;


    private void Start()
    {
        Debug.Log("Mars Climate Initiating!!!");

        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;
        tempMap = new float[heightMapResolution, heightMapResolution];
        humidityMap = new float[heightMapResolution, heightMapResolution];

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
                float humidity = Mathf.Lerp(minHumidity, maxHumidity, (yConversionUnit - (altitude*altFactor)) / yConversionUnit);

                tempMap[x, y] = temp;
                humidityMap[x, y] = humidity;
            }
        }

        Debug.Log("Done applying climate!!");
        //SaveMapsToCSV();
    }


    private int[] GetRelativeTerrainPosition(Vector3 roverPosition)
    {
        // converting rover's absolute position into relative position inside the terrain
        Vector3 terrainPos = marsTerrain.transform.InverseTransformDirection(roverPosition);

        int heightMapResolution = marsTerrain.terrainData.heightmapResolution;
        int x = Mathf.FloorToInt((terrainPos.x * heightMapResolution) / marsTerrain.terrainData.size.x);
        int y = Mathf.FloorToInt((terrainPos.z * heightMapResolution) / marsTerrain.terrainData.size.z); // z since it represents north-south in unity
        return new int[]{ x, y};
    }


    public float GetTemperatureAtPosition(Vector3 roverPosition)
    {
        int[] relativePosition = GetRelativeTerrainPosition(roverPosition);
        return tempMap[relativePosition[0], relativePosition[1]];
    }


    public float GetHumidityAtPosition(Vector3 roverPosition)
    {
        int[] relativePosition = GetRelativeTerrainPosition(roverPosition);
        return humidityMap[relativePosition[0], relativePosition[1]];
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

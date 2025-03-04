using UnityEngine;

public class MarsClimate : MonoBehaviour
{
    public Terrain marsTerrain;
    public float maxTemp = 20f;
    public float minTemp = -100f;
    public float maxHumidty = 0.0003f;
    public float minHumidty = 0f;
    public float altFactor = 0.2f; // altitude factor to simulate temp drop with latitude

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ApplyClimate();
    }

    // Update is called once per frame
    void ApplyClimate()
    {
        int heightMapResolution = marsTerrain.terrainData.heightmapResolution; 
        float[,] heights = marsTerrain.terrainData.GetHeights(0, 0, heightMapResolution, heightMapResolution);
        float[,] tempMap = new float[heightMapResolution, heightMapResolution];
        float[,] humidityMap = new float[heightMapResolution, heightMapResolution];

        for (int x = 0; x < heightMapResolution; x++)
        {
            for (int y = 0; y < heightMapResolution; y++)
            {
                float altitude = heights[x, y] * marsTerrain.terrainData.size.y; // convert to meters
                float temp = Mathf.Lerp(maxTemp, minTemp, (altitude*altFactor)/marsTerrain.terrainData.size.y); // interpolated value for this particular altitude   

                // Add variability based on latitude
                float latitudeFactor = Mathf.Abs(y / (float)heightMapResolution - 0.5f); // Latitude-based effect
            }
        }


    }
}

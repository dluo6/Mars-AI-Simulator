using UnityEngine;
using System.Collections.Generic;

public class GenerateWetAreas : MonoBehaviour
{
    public Terrain targetTerrain;
    public int darkTextureIndex = 1;
    public float paintRadius = 500f;

    private readonly List<Vector3> paintCenters = new()
    {
        new Vector3(48230.5f, 0f, 31548.2f),
        new Vector3(13845.7f, 0f, 44719.3f),
        new Vector3(42175.3f, 0f, 35982.1f),
        new Vector3(30592.8f, 0f, 10237.6f),
        new Vector3(5723.1f, 0f, 27485.4f),
        new Vector3(37984.5f, 0f, 49821.0f),
        new Vector3(21847.2f, 0f, 38267.9f),
        new Vector3(44312.0f, 0f, 17584.3f),
        new Vector3(15673.4f, 0f, 25419.7f),
        new Vector3(23077.3f, 0f, 18607.9f)
    };

    void Start()
    {
        PaintWetAreas();
    }

    public void PaintWetAreas()
    {
        if (targetTerrain == null) return;

        TerrainData terrainData = targetTerrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        // Get the current alpha map
        float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, width, height);

        foreach (Vector3 paintCenter in paintCenters)
        {
            // Convert to alpha map index
            float alphaX = (paintCenter.x / terrainData.size.x) * width;
            float alphaZ = (paintCenter.z / terrainData.size.z) * height;
            float alphaRadius = (paintRadius / terrainData.size.x) * width;

            // Paint a circle at each coordinate
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(alphaX, alphaZ));
                    if (dist < alphaRadius)
                    {
                        // Set weights so that only the dark texture is active
                        for (int i = 0; i < layers; i++)
                            alphaMap[y, x, i] = 0f;

                        alphaMap[y, x, darkTextureIndex] = 1f;
                    }
                }
            }
        }

        // Apply the modified alpha map
        terrainData.SetAlphamaps(0, 0, alphaMap);
    }
}

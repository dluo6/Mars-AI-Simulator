// GenerateRandomWaterSources() picks random (x, z) coordinates between paintRadius 
// and (terrainWidth - paintRadius) so each wet circle is fully contained within the terrain boundaries.

// PaintWetAreas() carries out the alpha painting logic.

// Adjust the following fields:
// numberOfWetAreas
// paintRadius


using UnityEngine;
using System.Collections.Generic;

public class GenerateWetAreas : MonoBehaviour
{
    public Terrain targetTerrain;

    // Index of the dark texture
    public int darkTextureIndex = 1;

    // Radius of the painted wet area
    public float paintRadius = 500f;

    // number of random wet areas to create
    public int numberOfWetAreas = 15;

    // We’ll fill this list at runtime with random points
    public static List<Vector3> WaterSources = new List<Vector3>();

    void Start()
    {
        GenerateRandomWaterSources();
        PaintWetAreas();
    }

    private void GenerateRandomWaterSources()
    {
        // Clear any existing points
        WaterSources.Clear();

        if (targetTerrain == null)
        {
            Debug.LogWarning("No terrain assigned to GenerateWetAreas!");
            return;
        }

        // The terrain’s usable XZ size is 0..50000
        // We must stay paintRadius away from each boundary
        // so we don’t paint off the edges
        float min = paintRadius;
        float max = targetTerrain.terrainData.size.x - paintRadius; // e.g. 50000 - 500 = 49500

        for (int i = 0; i < numberOfWetAreas; i++)
        {
            float randomX = Random.Range(min, max);
            float randomZ = Random.Range(min, max);
            WaterSources.Add(new Vector3(randomX, 0f, randomZ));
        }
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

        // Use the WaterSources we just randomly created
        foreach (Vector3 paintCenter in WaterSources)
        {
            // Convert world coordinates to alpha map indices
            int alphaX = Mathf.RoundToInt((paintCenter.x / terrainData.size.x) * width);
            int alphaZ = Mathf.RoundToInt((paintCenter.z / terrainData.size.z) * height);
            int alphaRadius = Mathf.RoundToInt((paintRadius / terrainData.size.x) * width);

            int minX = Mathf.Max(0, alphaX - alphaRadius);
            int maxX = Mathf.Min(width - 1, alphaX + alphaRadius);
            int minY = Mathf.Max(0, alphaZ - alphaRadius);
            int maxY = Mathf.Min(height - 1, alphaZ + alphaRadius);

            // Paint a circle of the "wet" texture
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float dist = Mathf.Sqrt((x - alphaX) * (x - alphaX) + (y - alphaZ) * (y - alphaZ));
                    if (dist < alphaRadius)
                    {
                        for (int i = 0; i < layers; i++)
                            alphaMap[y, x, i] = 0f;

                        // Paint the dark texture
                        alphaMap[y, x, darkTextureIndex] = 1f;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMap);
    }
}
// using UnityEngine;
// using System.Collections.Generic;

// public class GenerateWetAreas : MonoBehaviour
// {
//     public Terrain targetTerrain;
//     public int darkTextureIndex = 1;
//     public float paintRadius = 500f;

//     private readonly List<Vector3> paintCentersOG = new()
//     {
//         new Vector3(48230.5f, 0f, 31548.2f),
//         new Vector3(13845.7f, 0f, 44719.3f),
//         new Vector3(42175.3f, 0f, 35982.1f),
//         new Vector3(30592.8f, 0f, 10237.6f),
//         new Vector3(5723.1f, 0f, 27485.4f),
//         new Vector3(37984.5f, 0f, 49821.0f),
//         new Vector3(21847.2f, 0f, 38267.9f),
//         new Vector3(44312.0f, 0f, 17584.3f),
//         new Vector3(15673.4f, 0f, 25419.7f),
//         new Vector3(23077.3f, 0f, 18607.9f)
//     };

//     private readonly List<Vector3> paintCenters2 = new()
//     {
//         new Vector3(12043.2f, 0f, 39481.9f),
//         new Vector3(24513.8f, 0f, 4873.6f),
//         new Vector3(36094.1f, 0f, 27461.7f),
//         new Vector3(19831.5f, 0f, 43120.4f),
//         new Vector3(47293.2f, 0f, 16349.5f),
//         new Vector3(5124.7f, 0f, 33182.6f),
//         new Vector3(43917.6f, 0f, 48612.0f),
//         new Vector3(16324.9f, 0f, 22153.1f),
//         new Vector3(38612.5f, 0f, 31847.4f),
//         new Vector3(30214.7f, 0f, 9421.2f)
//     };

//     private readonly List<Vector3> paintCenters3 = new()
//     {
//         new Vector3(3901.3f, 0f, 10328.4f),
//         new Vector3(14423.9f, 0f, 28631.6f),
//         new Vector3(27682.1f, 0f, 47829.3f),
//         new Vector3(34213.8f, 0f, 36102.9f),
//         new Vector3(49620.5f, 0f, 18420.7f),
//         new Vector3(22893.4f, 0f, 39183.8f),
//         new Vector3(9052.7f, 0f, 4728.6f),
//         new Vector3(19485.6f, 0f, 21845.9f),
//         new Vector3(43172.2f, 0f, 12834.3f),
//         new Vector3(31192.6f, 0f, 45391.1f)
//     };

//     private readonly List<Vector3> paintCenters4 = new()
//     {
//         new Vector3(8123.5f, 0f, 31749.6f),
//         new Vector3(17921.6f, 0f, 42218.9f),
//         new Vector3(38625.4f, 0f, 25410.2f),
//         new Vector3(24812.9f, 0f, 14927.4f),
//         new Vector3(48123.8f, 0f, 8913.7f),
//         new Vector3(12983.6f, 0f, 32174.5f),
//         new Vector3(37952.4f, 0f, 47102.3f),
//         new Vector3(15612.8f, 0f, 10493.1f),
//         new Vector3(22847.1f, 0f, 19284.6f),
//         new Vector3(45728.3f, 0f, 36348.0f)
//     };

//     private readonly List<Vector3> paintCenters = new()
//     {
//         new Vector3(37412.2f, 0f, 21948.6f),
//         new Vector3(11349.5f, 0f, 46182.4f),
//         new Vector3(28472.9f, 0f, 13742.7f),
//         new Vector3(44210.3f, 0f, 34319.5f),
//         new Vector3(31984.6f, 0f, 48723.1f),
//         new Vector3(5832.4f, 0f, 23984.2f),
//         new Vector3(20738.1f, 0f, 16824.5f),
//         new Vector3(39104.6f, 0f, 20349.3f),
//         new Vector3(14321.9f, 0f, 27531.8f),
//         new Vector3(47629.4f, 0f, 11283.2f)
//     };

//     void Start()
//     {
//         PaintWetAreas();
//     }

//     public void PaintWetAreas()
//     {
//         if (targetTerrain == null) return;

//         TerrainData terrainData = targetTerrain.terrainData;
//         int width = terrainData.alphamapWidth;
//         int height = terrainData.alphamapHeight;
//         int layers = terrainData.alphamapLayers;

//         // Get the current alpha map
//         float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, width, height);

//         foreach (Vector3 paintCenter in paintCenters)
//         {
//             // Convert to alpha map index
//             int alphaX = Mathf.RoundToInt((paintCenter.x / terrainData.size.x) * width);
//             int alphaZ = Mathf.RoundToInt((paintCenter.z / terrainData.size.z) * height);
//             int alphaRadius = Mathf.RoundToInt((paintRadius / terrainData.size.x) * width);

//             // Limit search range to avoid unnecessary checks
//             int minX = Mathf.Max(0, alphaX - alphaRadius);
//             int maxX = Mathf.Min(width - 1, alphaX + alphaRadius);
//             int minY = Mathf.Max(0, alphaZ - alphaRadius);
//             int maxY = Mathf.Min(height - 1, alphaZ + alphaRadius);

//             for (int y = minY; y <= maxY; y++)
//             {
//                 for (int x = minX; x <= maxX; x++)
//                 {
//                     float dist = Mathf.Sqrt((x - alphaX) * (x - alphaX) + (y - alphaZ) * (y - alphaZ));
//                     if (dist < alphaRadius)
//                     {
//                         // Ensure only the dark texture is active
//                         for (int i = 0; i < layers; i++)
//                             alphaMap[y, x, i] = 0f;

//                         alphaMap[y, x, darkTextureIndex] = 1f;
//                     }
//                 }
//             }
//         }

//         // Apply the modified alpha map
//         terrainData.SetAlphamaps(0, 0, alphaMap);
//     }

// }

using UnityEngine;
using System.Collections.Generic;

public class GenerateWetAreas : MonoBehaviour
{
    public Terrain targetTerrain;
    public int darkTextureIndex = 1;
    public float paintRadius = 1000f;

    private List<Vector3> paintCenters;

    private readonly List<Vector3> paintCenters0 = new()
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

    private readonly List<Vector3> paintCenters1 = new()
    {
        new Vector3(12043.2f, 0f, 39481.9f),
        new Vector3(24513.8f, 0f, 4873.6f),
        new Vector3(36094.1f, 0f, 27461.7f),
        new Vector3(19831.5f, 0f, 43120.4f),
        new Vector3(47293.2f, 0f, 16349.5f),
        new Vector3(5124.7f, 0f, 33182.6f),
        new Vector3(43917.6f, 0f, 48612.0f),
        new Vector3(16324.9f, 0f, 22153.1f),
        new Vector3(38612.5f, 0f, 31847.4f),
        new Vector3(30214.7f, 0f, 9421.2f)
    };

    private readonly List<Vector3> paintCenters2 = new()
    {
        new Vector3(3901.3f, 0f, 10328.4f),
        new Vector3(14423.9f, 0f, 28631.6f),
        new Vector3(27682.1f, 0f, 47829.3f),
        new Vector3(34213.8f, 0f, 36102.9f),
        new Vector3(49620.5f, 0f, 18420.7f),
        new Vector3(22893.4f, 0f, 39183.8f),
        new Vector3(9052.7f, 0f, 4728.6f),
        new Vector3(19485.6f, 0f, 21845.9f),
        new Vector3(43172.2f, 0f, 12834.3f),
        new Vector3(31192.6f, 0f, 45391.1f)
    };

    private readonly List<Vector3> paintCenters3 = new()
    {
        new Vector3(8123.5f, 0f, 31749.6f),
        new Vector3(17921.6f, 0f, 42218.9f),
        new Vector3(38625.4f, 0f, 25410.2f),
        new Vector3(24812.9f, 0f, 14927.4f),
        new Vector3(48123.8f, 0f, 8913.7f),
        new Vector3(12983.6f, 0f, 32174.5f),
        new Vector3(37952.4f, 0f, 47102.3f),
        new Vector3(15612.8f, 0f, 10493.1f),
        new Vector3(22847.1f, 0f, 19284.6f),
        new Vector3(45728.3f, 0f, 36348.0f)
    };

    private readonly List<Vector3> paintCenters4 = new()
    {
        new Vector3(37412.2f, 0f, 21948.6f),
        new Vector3(11349.5f, 0f, 46182.4f),
        new Vector3(28472.9f, 0f, 13742.7f),
        new Vector3(44210.3f, 0f, 34319.5f),
        new Vector3(31984.6f, 0f, 48723.1f),
        new Vector3(5832.4f, 0f, 23984.2f),
        new Vector3(20738.1f, 0f, 16824.5f),
        new Vector3(39104.6f, 0f, 20349.3f),
        new Vector3(14321.9f, 0f, 27531.8f),
        new Vector3(47629.4f, 0f, 11283.2f)
    };

    void Start()
    {
        SelectConfig(WaterConfigSelection.SelectedConfigIndex);
        PaintWetAreas();
    }

    void SelectConfig(int index)
    {
        switch (index)
        {
            case 0: paintCenters = paintCenters0; break;
            case 1: paintCenters = paintCenters1; break;
            case 2: paintCenters = paintCenters2; break;
            case 3: paintCenters = paintCenters3; break;
            case 4: paintCenters = paintCenters4; break;
            default: paintCenters = paintCenters0; break;
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

        foreach (Vector3 paintCenter in paintCenters)
        {
            // Convert to alpha map index
            int alphaX = Mathf.RoundToInt((paintCenter.x / terrainData.size.x) * width);
            int alphaZ = Mathf.RoundToInt((paintCenter.z / terrainData.size.z) * height);
            int alphaRadius = Mathf.RoundToInt((paintRadius / terrainData.size.x) * width);

            // Limit search range to avoid unnecessary checks
            int minX = Mathf.Max(0, alphaX - alphaRadius);
            int maxX = Mathf.Min(width - 1, alphaX + alphaRadius);
            int minY = Mathf.Max(0, alphaZ - alphaRadius);
            int maxY = Mathf.Min(height - 1, alphaZ + alphaRadius);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float dist = Mathf.Sqrt((x - alphaX) * (x - alphaX) + (y - alphaZ) * (y - alphaZ));
                    if (dist < alphaRadius)
                    {
                        // Ensure only the dark texture is active
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

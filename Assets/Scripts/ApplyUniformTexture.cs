using UnityEngine;

public class ApplyUniformTexture : MonoBehaviour
{
    [Header("Terrain and Layer Settings")]
    public Terrain targetTerrain;

    [Tooltip("Index of the texture you want to use everywhere. " +
             "Check your Terrain Layers in the Terrain Inspector and use 0-based indexing.")]
    public int textureIndex = 0;

    [ContextMenu("Paint Entire Terrain Uniformly")]
    public void PaintAllSameTexture()
    {
        if (targetTerrain == null)
        {
            Debug.LogError("No Terrain assigned.");
            return;
        }

        TerrainData terrainData = targetTerrain.terrainData;
        int alphaMapWidth = terrainData.alphamapWidth;
        int alphaMapHeight = terrainData.alphamapHeight;
        int alphaMapLayers = terrainData.alphamapLayers;

        // Just for clarity, get the layers:
        TerrainLayer[] layers = terrainData.terrainLayers;

        // Safety check
        if (textureIndex < 0 || textureIndex >= layers.Length)
        {
            Debug.LogError($"textureIndex {textureIndex} is out of range. " +
                           $"The terrain only has {layers.Length} layers.");
            return;
        }

        // Create a new alpha map array
        float[,,] newAlphaMap = new float[alphaMapHeight, alphaMapWidth, alphaMapLayers];

        // We want everything 0 except the chosen texture index = 1
        for (int y = 0; y < alphaMapHeight; y++)
        {
            for (int x = 0; x < alphaMapWidth; x++)
            {
                for (int layer = 0; layer < alphaMapLayers; layer++)
                {
                    newAlphaMap[y, x, layer] = 0f;
                }
                newAlphaMap[y, x, textureIndex] = 1f;
            }
        }

        // Apply the uniform alpha map
        terrainData.SetAlphamaps(0, 0, newAlphaMap);

        // Print the name of the layer
        string layerName = layers[textureIndex].name;
        Debug.Log($"Painted entire terrain with layer '{layerName}'.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            PaintAllSameTexture();
        }
    }
}

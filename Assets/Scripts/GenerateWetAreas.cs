using UnityEngine;

public class GenerateWetAreas : MonoBehaviour
{
    public Terrain targetTerrain;
    public int darkTextureIndex = 1;
    public float paintRadius = 100f;
    public Vector3 paintCenter;

    private float[,,] originalAlphaMap; // backup

    void Awake()
    {
        if (targetTerrain != null)
        {
            // Store original alpha map
            var terrainData = targetTerrain.terrainData;
            int width = terrainData.alphamapWidth;
            int height = terrainData.alphamapHeight;
            originalAlphaMap = terrainData.GetAlphamaps(0, 0, width, height);
        }
    }

    public void PaintTerrainDark()
    {
        if (targetTerrain == null) return;

        TerrainData terrainData = targetTerrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        // Get the current alpha map
        float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, width, height);

        // Convert to alpha map index
        float alphaX = (paintCenter.x / terrainData.size.x) * width;
        float alphaZ = (paintCenter.z / terrainData.size.z) * height;
        float alphaRadius = (paintRadius / terrainData.size.x) * width;

        // Paint a circle
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(alphaX, alphaZ));
                if (dist < alphaRadius)
                {
                    // Set weights so that only dark texture is active
                    for (int i = 0; i < layers; i++)
                        alphaMap[y, x, i] = 0f;

                    alphaMap[y, x, darkTextureIndex] = 1f;
                }
            }
        }

        // Apply the modified alpha map
        terrainData.SetAlphamaps(0, 0, alphaMap);
    }

    // Revert changes by reapplying the original alpha map
    public void RevertPaint()
    {
        if (targetTerrain == null || originalAlphaMap == null) return;
        TerrainData terrainData = targetTerrain.terrainData;
        terrainData.SetAlphamaps(0, 0, originalAlphaMap);
        Debug.Log("Terrain painting reverted.");
    }

    void Update()
    {
        // Example: press 'P' to paint, press 'R' to revert
        if (Input.GetKeyDown(KeyCode.P))
        {
            PaintTerrainDark();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RevertPaint();
        }
    }

}

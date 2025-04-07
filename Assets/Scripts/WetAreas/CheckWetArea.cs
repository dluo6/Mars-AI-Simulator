using UnityEngine;

public class CheckWetArea : MonoBehaviour
{
    [SerializeField] Terrain terrain;
    [SerializeField] int wetLayerIndex = 1;
    [SerializeField] float wetThreshold = 0.3f;

    [Header("Audio Settings")]
    [SerializeField] AudioSource waterAudioSource;

    float checkInterval = 1.0f;
    float nextCheckTime = 0f;
    bool isPlayingWaterSound = false;

    TerrainData terrainData;
    Vector3 terrainPos;

    // Public property to let other scripts know if we're on wet terrain.
    public bool IsOnWetArea { get; private set; }

    void Start()
    {
        if (terrain == null)
        {
            terrain = FindFirstObjectByType<Terrain>();
        }
        terrainData = terrain.terrainData;
        terrainPos = terrain.GetPosition();
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            IsOnWetArea = CheckIfOnWetArea();
            nextCheckTime = Time.time + checkInterval;
            HandleWaterSound(IsOnWetArea);
        }
    }

    bool CheckIfOnWetArea()
    {
        Vector3 charPos = transform.position; // The rover's position
        float relativeX = (charPos.x - terrainPos.x);
        float relativeZ = (charPos.z - terrainPos.z);

        int mapX = Mathf.RoundToInt((relativeX / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((relativeZ / terrainData.size.z) * terrainData.alphamapHeight);

        // Clamp to valid range
        mapX = Mathf.Clamp(mapX, 0, terrainData.alphamapWidth - 1);
        mapZ = Mathf.Clamp(mapZ, 0, terrainData.alphamapHeight - 1);

        // Get the splatmap data
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        float wetWeight = splatmapData[0, 0, wetLayerIndex];

        if (wetWeight > wetThreshold)
        {
            Debug.Log("Rover is on wet terrain!");
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleWaterSound(bool isOnWet)
    {
        if (isOnWet)
        {
            if (waterAudioSource != null && !isPlayingWaterSound)
            {
                waterAudioSource.Play();
                isPlayingWaterSound = true;
            }
        }
        else
        {
            if (waterAudioSource != null && isPlayingWaterSound)
            {
                waterAudioSource.Stop();
                isPlayingWaterSound = false;
            }
        }
    }
}
// using UnityEngine;

// public class CheckWetArea : MonoBehaviour
// {
//     [SerializeField] Terrain terrain;
//     [SerializeField] int wetLayerIndex = 1; // The index for your wet layer in the Terrain Layers
//     [SerializeField] float wetThreshold = 0.3f;

//     float checkInterval = 1.0f;
//     float nextCheckTime = 0f;

//     TerrainData terrainData;
//     Vector3 terrainPos;

//     void Start()
//     {
//         if (terrain == null)
//         {
//             terrain = FindFirstObjectByType<Terrain>();
//         }
//         terrainData = terrain.terrainData;
//         terrainPos = terrain.GetPosition();
//     }

//     void Update()
//     {
//         if (Time.time >= nextCheckTime)
//         {
//             CheckIfOnWetArea();
//             nextCheckTime = Time.time + checkInterval;
//         }
//     }

//     void CheckIfOnWetArea()
//     {
//         Vector3 charPos = transform.position; // The character's position
//         float relativeX = (charPos.x - terrainPos.x);
//         float relativeZ = (charPos.z - terrainPos.z);

//         int mapX = Mathf.RoundToInt((relativeX / terrainData.size.x) * terrainData.alphamapWidth);
//         int mapZ = Mathf.RoundToInt((relativeZ / terrainData.size.z) * terrainData.alphamapHeight);

//         // Clamp to valid range
//         mapX = Mathf.Clamp(mapX, 0, terrainData.alphamapWidth - 1);
//         mapZ = Mathf.Clamp(mapZ, 0, terrainData.alphamapHeight - 1);

//         // Get the splatmap data
//         float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
//         float wetWeight = splatmapData[0, 0, wetLayerIndex];

//         if (wetWeight > wetThreshold)
//         {
//             Debug.Log("Character is on wet terrain!");
//             // Here you can call a function that triggers a specific task
//             // e.g. Play a unique footstep sound or reduce movement speed, etc.
//         }
//         else
//         {
//             Debug.Log("Character is NOT on wet terrain.");
//         }
//     }
// }

// using UnityEngine;
// using UnityEngine.UI;

// public class Minimap : MonoBehaviour
// {
//     [SerializeField] private Transform rover; // The rover to track
//     [SerializeField] private Transform terrain; // Reference to terrain

//     [Header("Minimap Settings")]
//     [SerializeField] private float minimapSize = 150f; // Size of the minimap in UI pixels
//     [SerializeField] private float cameraHeight = 1000f; // Height of camera above terrain
//     [SerializeField] private float markerSize = 10f; // Size of the rover marker
//     [SerializeField] private Color markerColor = new Color32(0xF5, 0xFF, 0x01, 0xFF);


//     private Camera minimapCamera;
//     private RenderTexture minimapTexture;
//     private RawImage minimapImage;
//     private RectTransform minimapRect;
//     private RectTransform roverMarker;

//     void Start()
//     {
//         // Create minimap camera
//         GameObject minimapCamObj = new GameObject("MinimapCamera");
//         minimapCamera = minimapCamObj.AddComponent<Camera>();
//         minimapCamera.orthographic = true;

//         // set camera to cover the entire terrain
//         if (terrain != null)
//         {
//             Terrain terrainComponent = terrain.GetComponent<Terrain>();
//             if (terrainComponent != null)
//             {
//                 // Get terrain size
//                 Vector3 terrainSize = terrainComponent.terrainData.size;

//                 // Position camera above center of terrain
//                 Vector3 terrainCenter = terrain.position + new Vector3(terrainSize.x / 2, 0, terrainSize.z / 2);
//                 minimapCamera.transform.position = new Vector3(terrainCenter.x, cameraHeight, terrainCenter.z);

//                 // Set orthographic size to fit terrain width or height, whichever is larger
//                 minimapCamera.orthographicSize = Mathf.Max(terrainSize.x, terrainSize.z) / 2;
//             }
//             else
//             {
//                 // Fallback for generic transforms
//                 minimapCamera.transform.position = new Vector3(terrain.position.x, cameraHeight, terrain.position.z);
//                 minimapCamera.orthographicSize = 100f; // Default size
//             }
//         }
//         else
//         {
//             // Default positioning if no terrain is specified
//             minimapCamera.transform.position = new Vector3(0, cameraHeight, 0);
//             minimapCamera.orthographicSize = 100f; // Default size
//         }

//         minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Look straight down
//         minimapCamera.cullingMask = ~(1 << LayerMask.NameToLayer("UI")); // Don't render UI

//         // Create render texture
//         minimapTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
//         minimapCamera.targetTexture = minimapTexture;

//         // Create UI components
//         GameObject canvasObj = new GameObject("MinimapCanvas");
//         Canvas canvas = canvasObj.AddComponent<Canvas>();
//         canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//         canvasObj.AddComponent<CanvasScaler>();
//         canvasObj.AddComponent<GraphicRaycaster>();

//         // Create minimap image
//         GameObject minimapObj = new GameObject("Minimap");
//         minimapObj.transform.SetParent(canvasObj.transform, false);
//         minimapImage = minimapObj.AddComponent<RawImage>();
//         minimapImage.texture = minimapTexture;
//         minimapRect = minimapImage.rectTransform;

//         // Set to top-right corner
//         minimapRect.anchorMin = new Vector2(1, 1);
//         minimapRect.anchorMax = new Vector2(1, 1);
//         minimapRect.pivot = new Vector2(1, 1);
//         minimapRect.anchoredPosition = new Vector2(-10, -10);
//         minimapRect.sizeDelta = new Vector2(minimapSize, minimapSize);

//         // Create rover marker
//         GameObject markerObj = new GameObject("RoverMarker");
//         markerObj.transform.SetParent(minimapObj.transform, false);
//         Image markerImage = markerObj.AddComponent<Image>();

//         // Make marker a circle
//         markerImage.sprite = CreateCircleSprite(32);
//         markerImage.color = markerColor;

//         roverMarker = markerImage.rectTransform;
//         roverMarker.sizeDelta = new Vector2(markerSize, markerSize);

//         // Add border to minimap
//         GameObject borderObj = new GameObject("MinimapBorder");
//         borderObj.transform.SetParent(canvasObj.transform, false);
//         Image borderImage = borderObj.AddComponent<Image>();
//         borderImage.color = new Color(1, 1, 1, 0.5f);
//         RectTransform borderRect = borderImage.rectTransform;
//         borderRect.anchorMin = minimapRect.anchorMin;
//         borderRect.anchorMax = minimapRect.anchorMax;
//         borderRect.pivot = minimapRect.pivot;
//         borderRect.anchoredPosition = minimapRect.anchoredPosition;
//         borderRect.sizeDelta = minimapRect.sizeDelta + new Vector2(4, 4);
//         borderObj.transform.SetAsFirstSibling(); // Put border behind minimap
//     }

//     void LateUpdate()
//     {
//         if (rover == null)
//         {
//             Debug.LogWarning("Rover reference is missing. Please assign it in the Inspector.");
//             return;
//         }

//         // Convert rover's world position to minimap position
//         Vector2 roverNormalizedPos = GetNormalizedPosition(rover.position);

//         // Map normalized position (0-1) to minimap coordinates
//         Vector2 roverMinimapPos = new Vector2(
//             roverNormalizedPos.x * minimapSize,
//             roverNormalizedPos.y * minimapSize
//         );

//         // Position the marker - remember (0,0) is bottom-left in the minimap space
//         roverMarker.anchorMin = new Vector2(0, 0);
//         roverMarker.anchorMax = new Vector2(0, 0);
//         roverMarker.pivot = new Vector2(0.5f, 0.5f); // Center the marker on the rover position
//         roverMarker.anchoredPosition = roverMinimapPos;
//     }

//     // Convert world position to normalized position (0-1) on the minimap
//     private Vector2 GetNormalizedPosition(Vector3 worldPos)
//     {
//         if (terrain != null)
//         {
//             Terrain terrainComponent = terrain.GetComponent<Terrain>();
//             if (terrainComponent != null)
//             {
//                 // Get terrain boundaries
//                 Vector3 terrainPos = terrain.position;
//                 Vector3 terrainSize = terrainComponent.terrainData.size;

//                 // Calculate position relative to terrain
//                 float normalizedX = (worldPos.x - terrainPos.x) / terrainSize.x;
//                 float normalizedZ = (worldPos.z - terrainPos.z) / terrainSize.z;

//                 return new Vector2(normalizedX, normalizedZ);
//             }
//         }

//         // Fallback using camera boundaries
//         Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(worldPos);
//         return new Vector2(viewportPoint.x, viewportPoint.y);
//     }

//     // Create a circle sprite for the rover marker
//     private Sprite CreateCircleSprite(int resolution)
//     {
//         Texture2D texture = new Texture2D(resolution, resolution);

//         float centerX = resolution / 2f;
//         float centerY = resolution / 2f;
//         float radius = resolution / 2f;

//         for (int x = 0; x < resolution; x++)
//         {
//             for (int y = 0; y < resolution; y++)
//             {
//                 float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
//                 if (distance < radius)
//                 {
//                     texture.SetPixel(x, y, Color.white);
//                 }
//                 else
//                 {
//                     texture.SetPixel(x, y, Color.clear);
//                 }
//             }
//         }

//         texture.Apply();

//         return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
//     }
// }

using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform terrain; // Reference to terrain

    [Header("Minimap Settings")]
    [SerializeField] private float minimapSize = 150f; // Size of the minimap in UI pixels
    [SerializeField] private float cameraHeight = 1000f; // Height of camera above terrain
    [SerializeField] private float markerSize = 10f; // Size of the rover marker
    [SerializeField] private Color markerColor = new Color32(0xF5, 0xFF, 0x01, 0xFF);

    private Camera minimapCamera;
    private RenderTexture minimapTexture;
    private RawImage minimapImage;
    private RectTransform minimapRect;
    private RectTransform roverMarker;

    void Start()
    {
        // Create minimap camera
        GameObject minimapCamObj = new GameObject("MinimapCamera");
        minimapCamera = minimapCamObj.AddComponent<Camera>();
        minimapCamera.orthographic = true;

        // set camera to cover the entire terrain
        if (terrain != null)
        {
            Terrain terrainComponent = terrain.GetComponent<Terrain>();
            if (terrainComponent != null)
            {
                // Get terrain size
                Vector3 terrainSize = terrainComponent.terrainData.size;

                // Position camera above center of terrain
                Vector3 terrainCenter = terrain.position + new Vector3(terrainSize.x / 2, 0, terrainSize.z / 2);
                minimapCamera.transform.position = new Vector3(terrainCenter.x, cameraHeight, terrainCenter.z);

                // Set orthographic size to fit terrain width or height, whichever is larger
                minimapCamera.orthographicSize = Mathf.Max(terrainSize.x, terrainSize.z) / 2;
            }
            else
            {
                // Fallback for generic transforms
                minimapCamera.transform.position = new Vector3(terrain.position.x, cameraHeight, terrain.position.z);
                minimapCamera.orthographicSize = 100f; // Default size
            }
        }
        else
        {
            // Default positioning if no terrain is specified
            minimapCamera.transform.position = new Vector3(0, cameraHeight, 0);
            minimapCamera.orthographicSize = 100f; // Default size
        }

        minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Look straight down
        minimapCamera.cullingMask = ~(1 << LayerMask.NameToLayer("UI")); // Don't render UI

        // Create render texture
        minimapTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        minimapCamera.targetTexture = minimapTexture;

        // Create UI components
        GameObject canvasObj = new GameObject("MinimapCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create minimap image
        GameObject minimapObj = new GameObject("Minimap");
        minimapObj.transform.SetParent(canvasObj.transform, false);
        minimapImage = minimapObj.AddComponent<RawImage>();
        minimapImage.texture = minimapTexture;
        minimapRect = minimapImage.rectTransform;

        // Set to top-right corner
        minimapRect.anchorMin = new Vector2(1, 1);
        minimapRect.anchorMax = new Vector2(1, 1);
        minimapRect.pivot = new Vector2(1, 1);
        minimapRect.anchoredPosition = new Vector2(-10, -10);
        minimapRect.sizeDelta = new Vector2(minimapSize, minimapSize);

        // Create rover marker
        GameObject markerObj = new GameObject("RoverMarker");
        markerObj.transform.SetParent(minimapObj.transform, false);
        Image markerImage = markerObj.AddComponent<Image>();

        // Make marker a circle
        markerImage.sprite = CreateCircleSprite(32);
        markerImage.color = markerColor;

        roverMarker = markerImage.rectTransform;
        roverMarker.sizeDelta = new Vector2(markerSize, markerSize);

        // Add border to minimap
        GameObject borderObj = new GameObject("MinimapBorder");
        borderObj.transform.SetParent(canvasObj.transform, false);
        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = new Color(1, 1, 1, 0.5f);
        RectTransform borderRect = borderImage.rectTransform;
        borderRect.anchorMin = minimapRect.anchorMin;
        borderRect.anchorMax = minimapRect.anchorMax;
        borderRect.pivot = minimapRect.pivot;
        borderRect.anchoredPosition = minimapRect.anchoredPosition;
        borderRect.sizeDelta = minimapRect.sizeDelta + new Vector2(4, 4);
        borderObj.transform.SetAsFirstSibling(); // Put border behind minimap
    }

    void LateUpdate()
    {
        // Get the current rover from GlobalVariables
        Transform currentRover = GetCurrentRoverTransform();

        if (currentRover == null)
        {
            // Hide the marker if no rover is available
            if (roverMarker != null)
                roverMarker.gameObject.SetActive(false);
            return;
        }
        else
        {
            // Show the marker
            if (roverMarker != null && !roverMarker.gameObject.activeSelf)
                roverMarker.gameObject.SetActive(true);
        }

        // Convert rover's world position to minimap position
        Vector2 roverNormalizedPos = GetNormalizedPosition(currentRover.position);

        // Map normalized position (0-1) to minimap coordinates
        Vector2 roverMinimapPos = new Vector2(
            roverNormalizedPos.x * minimapSize,
            roverNormalizedPos.y * minimapSize
        );

        // Position the marker - remember (0,0) is bottom-left in the minimap space
        roverMarker.anchorMin = new Vector2(0, 0);
        roverMarker.anchorMax = new Vector2(0, 0);
        roverMarker.pivot = new Vector2(0.5f, 0.5f); // Center the marker on the rover position
        roverMarker.anchoredPosition = roverMinimapPos;
    }

    // Helper method to get the current rover transform
    private Transform GetCurrentRoverTransform()
    {
        if (GlobalVariables.Instance == null)
        {
            Debug.LogWarning("GlobalVariables instance not found!");
            return null;
        }

        if (GlobalVariables.Instance.rovers == null || GlobalVariables.Instance.rovers.Count == 0)
        {
            Debug.LogWarning("No rovers in the GlobalVariables list!");
            return null;
        }

        int index = GlobalVariables.Instance.currentRoverIndex;

        // Make sure the index is valid
        if (index >= 0 && index < GlobalVariables.Instance.rovers.Count)
        {
            GameObject currentRover = GlobalVariables.Instance.rovers[index];
            if (currentRover != null)
            {
                return currentRover.transform;
            }
        }

        Debug.LogWarning("Current rover index is invalid or rover is null!");
        return null;
    }

    // Convert world position to normalized position (0-1) on the minimap
    private Vector2 GetNormalizedPosition(Vector3 worldPos)
    {
        if (terrain != null)
        {
            Terrain terrainComponent = terrain.GetComponent<Terrain>();
            if (terrainComponent != null)
            {
                // Get terrain boundaries
                Vector3 terrainPos = terrain.position;
                Vector3 terrainSize = terrainComponent.terrainData.size;

                // Calculate position relative to terrain
                float normalizedX = (worldPos.x - terrainPos.x) / terrainSize.x;
                float normalizedZ = (worldPos.z - terrainPos.z) / terrainSize.z;

                return new Vector2(normalizedX, normalizedZ);
            }
        }

        // Fallback using camera boundaries
        Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(worldPos);
        return new Vector2(viewportPoint.x, viewportPoint.y);
    }

    // Create a circle sprite for the rover marker
    private Sprite CreateCircleSprite(int resolution)
    {
        Texture2D texture = new Texture2D(resolution, resolution);

        float centerX = resolution / 2f;
        float centerY = resolution / 2f;
        float radius = resolution / 2f;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                if (distance < radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
    }
}
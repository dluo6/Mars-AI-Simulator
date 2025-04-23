using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MapToTerrain : MonoBehaviour, IPointerClickHandler
{
    public RectTransform map;
    public RectTransform confirmScreen;
    public Terrain terrain;
    public Transform cam;
    public Button startButton;
    public GameObject player;
    public GameObject targetPrefab;
    public TMP_InputField nameInput;
    private CanvasGroup mapCanvas;
    private CanvasGroup confirmScreenCanvas;
    private GameObject currentPlayer;
    private int curNumPlayers = 0;
    // This variable stores where on the map was clicked to place the target
    private Vector2 currentLocalPosition;

    public void Start()
    {
        startButton.gameObject.SetActive(false);
        mapCanvas = map.GetComponent<CanvasGroup>();
        confirmScreenCanvas = confirmScreen.GetComponent<CanvasGroup>();
        Hide(confirmScreenCanvas);
        // Freeze the game so that the rovers do not move
        Time.timeScale = 0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Remove ability to add new rovers
        if (curNumPlayers == GlobalVariables.Instance.numPlayers) return;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(map, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            // Convert local point to normalized coordinates (0 to 1)
            float normalizedX = (localPoint.x + map.rect.width / 2) / map.rect.width;
            float normalizedZ = (localPoint.y + map.rect.height / 2) / map.rect.height;

            // Convert to terrain world coordinates
            float worldX = normalizedX * terrain.terrainData.size.x;
            float worldZ = normalizedZ * terrain.terrainData.size.z;
            float worldY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ));

            currentLocalPosition = localPoint;
            // Move the player or camera
            MoveToLocation(new Vector3(worldX, worldY + 5, worldZ));
        }
    }

    public void MoveToLocation(Vector3 position)
    {
        Hide(mapCanvas);
        Show(confirmScreenCanvas);

        // Instantiate a new rover and show it on camera
        currentPlayer = Instantiate(player, position, new Quaternion(0, 0, 0, 0));
        RoverManager roverManager = currentPlayer.GetComponent<RoverManager>();
        GameObject activeRover = roverManager.CurrentActiveRover;

        Vector3 camPos = position + new Vector3(10, 20, -40);
        cam.position = camPos;
        cam.LookAt(activeRover.transform);
        cam.position += new Vector3(-20, 0, 0);
    }

    public void AddPlayer()
    {
        // Add the rover and its name to keep for the simulation itself
        RoverManager roverManager = currentPlayer.GetComponent<RoverManager>();
        if (nameInput.text != "")
        {
            roverManager.SetPlayerName(nameInput.text);
        }
        GlobalVariables.Instance.AddToList(currentPlayer);
        curNumPlayers += 1;

        // Add the target prefab to the canvas before showing
        GameObject target = Instantiate(targetPrefab, map);
        target.GetComponent<RectTransform>().anchoredPosition = currentLocalPosition;

        // Toggle the canvas visibilities and check to display the start button
        Hide(confirmScreenCanvas);
        Show(mapCanvas);
        if (curNumPlayers == GlobalVariables.Instance.numPlayers)
        {
            startButton.gameObject.SetActive(true);
        }
    }

    public void CancelAdding()
    {
        Hide(confirmScreenCanvas);
        Show(mapCanvas);
        Destroy(currentPlayer);
    }

    public void Hide(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;  // Make it invisible
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;  // Make it visible
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
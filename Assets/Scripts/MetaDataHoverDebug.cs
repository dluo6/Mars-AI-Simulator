using UnityEngine;

public class MetaDataHoverDebug : MonoBehaviour
{

    public Terrain marsTerrain;
    public Camera mainCamera;
    public MarsClimate marsClimate;


    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == marsTerrain.GetComponent<Collider>())
            {
                float temperature = marsClimate.GetTemperatureAtPosition(hit.point);
                Debug.Log("Temperature at this position: " + temperature);
            }
        }
    }
}

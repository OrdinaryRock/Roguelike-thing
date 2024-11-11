using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public static Minimap Instance { get; private set; }

    [SerializeField]
    private GameObject roomIconTemplate;
    [SerializeField]
    private GameObject hallIconTemplate;
    [SerializeField]
    private Vector2 iconSpacing = new Vector2(3,2);

    private List<Vector2> roomLocations;
    private List<Vector2> hallLocations;
    private Dictionary<Vector2, GameObject> roomIcons = new Dictionary<Vector2, GameObject>();
    private Dictionary<Vector2, GameObject> hallIcons = new Dictionary<Vector2, GameObject>();

    // Ensure we are the only instance of ourself
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void SetData(List<Vector2> iconLocations, List<Vector2> hallLocations)
    {
        this.roomLocations = iconLocations;
        this.hallLocations = hallLocations;

        InitializeGraphicDisplay();
    }

    public void InitializeGraphicDisplay()
    {
        // Hallways
        int i = 1;
        Vector2 l1 = Vector2.zero;
        Vector2 l2 = Vector2.zero;
        foreach(Vector2 location in hallLocations)
        {
            if(i == 1)
            {
                l1 = location;
                i = 2;
            }
            else if(i == 2)
            {
                l2 = location;
                Vector2 hallLocation = (l1 + l2) / 2;
                Vector2 spawnPosition = (Vector2) transform.position + hallLocation * iconSpacing;
                Quaternion spawnRotation = Quaternion.LookRotation(Vector3.forward, l2 - l1);
                GameObject iconInstance = Instantiate(hallIconTemplate, spawnPosition, spawnRotation, transform);
                iconInstance.SetActive(true);
                iconInstance.transform.Rotate(Vector3.forward, 90);
                hallIcons.Add(hallLocation, iconInstance);
                i = 1;
            }
        }
        // Rooms
        foreach(Vector2 roomLocation in roomLocations)
        {
            Vector2 spawnPosition = (Vector2) transform.position + roomLocation * iconSpacing;
            GameObject iconInstance = Instantiate(roomIconTemplate, spawnPosition, transform.rotation, transform);
            iconInstance.SetActive(true);
            roomIcons.Add(roomLocation, iconInstance);
        }
        
    }

    private void Update()
    {
        foreach(KeyValuePair<Vector2, GameObject> entry in roomIcons)
        {
            Vector2 roomLocation = entry.Key;
            GameObject icon = entry.Value;
            icon.transform.position = (Vector2) transform.position + roomLocation * iconSpacing;
        }
        foreach(KeyValuePair<Vector2, GameObject> entry in hallIcons)
        {
            Vector2 hallLocation = entry.Key;
            GameObject icon = entry.Value;
            icon.transform.position = (Vector2) transform.position + hallLocation * iconSpacing;
        }
    }


}

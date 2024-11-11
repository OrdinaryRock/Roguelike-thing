using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField]
    private DungeonDoor northDoor;
    [SerializeField]
    private DungeonDoor eastDoor;
    [SerializeField]
    private DungeonDoor southDoor;
    [SerializeField]
    private DungeonDoor westDoor;

    private Dictionary<Vector2, DungeonDoor> allDoors = new Dictionary<Vector2, DungeonDoor>();
    private Dictionary<Vector2, DungeonDoor> unlockedDoors = new Dictionary<Vector2, DungeonDoor>();
    private List<Vector2> roomConnections;
    private Vector2 roomCoordinates;

    public enum RoomType
    {
        Start, Boss, Treasure, Normal
    }
    private RoomType roomType;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomConnections(List<Vector2> connections)
    {
        roomConnections = connections;
    }

    public void SetRoomType(int type)
    {
        roomType = (RoomType) type;
    }

    public void SetRoomCoords(Vector2 coordinates)
    {
        roomCoordinates = coordinates;
    }

    public Vector2 GetRoomCoordinates()
    {
        return roomCoordinates;
    }

    // Start is called before the first frame update
    void Start()
    {
        allDoors.Add(Vector2.up, northDoor);
        allDoors.Add(Vector2.right, eastDoor);
        allDoors.Add(Vector2.down, southDoor);
        allDoors.Add(Vector2.left, westDoor);

        foreach(Vector2 direction in roomConnections)
        {
            allDoors[direction].SetLocked(false);
            /* if(roomType == RoomType.Start) */ allDoors[direction].SetOpen(true);
            unlockedDoors.Add(direction, allDoors[direction]);
        }
    }
}

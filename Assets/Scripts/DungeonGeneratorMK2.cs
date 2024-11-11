using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorMK2 : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private int desiredRoomCount = 15;

    enum ConnectionType
    {
        North, South, East, West
    }
    enum RoomType
    {
        Start, Boss, Treasure, Normal
    }

    private class RoomData
    {
        public RoomType roomType = RoomType.Normal;
        public List<Vector2> roomConnections = new List<Vector2>();
        public GameObject roomInstance;
        
        public RoomData() { }
        public RoomData(RoomType roomType)
        {
            this.roomType = roomType;
        }
    }

    private Dictionary<Vector2, RoomData> roomMap = new Dictionary<Vector2, RoomData>();
    private List<Vector2> minimapRoomLocations = new List<Vector2>();
    private List<Vector2> minimapHallLocations = new List<Vector2>();
    private Vector2 roomSpacing;


    // Start is called before the first frame update
    private void Start()
    {
        roomSpacing = roomPrefab.GetComponent<SpriteRenderer>().size;
        GenerateMap();
    }

    private void PurgeRoomMap()
    {
        foreach(KeyValuePair<Vector2, RoomData> entry in roomMap)
        {
            RoomData room = entry.Value;
            Destroy(room.roomInstance);
        }
        roomMap.Clear();
    }

    private bool CoordinatesAreTaken(Vector2 coordinates)
    {
        if(roomMap.ContainsKey(coordinates)) return true;
        else return false;
    }

    private void GenerateMap()
    {
        PurgeRoomMap();
        RoomData initialRoom = new RoomData(RoomType.Start);
        Vector2 initialCoordinates = Vector2.zero;
        roomMap.Add(initialCoordinates, initialRoom);
        minimapRoomLocations.Add(initialCoordinates);
        GenerateRoomPath(initialCoordinates, initialRoom, false);
        InstantiateRooms();
        PushMinimapData();
    }

    private void GenerateRoomPath(Vector2 actingCoordinates, RoomData actingRoom, bool isBranch)
    {
        List<Vector2> cardinalDirections = new List<Vector2>() {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        List<Vector2> possibleDirections = cardinalDirections;
        Vector2 nextDirection;

        int i = 0;
        int roomsLeftToFill;
        if(isBranch) roomsLeftToFill = Random.Range(1, 4);
        else roomsLeftToFill = desiredRoomCount - 1;
        while(roomsLeftToFill > 0)
        {
            if(possibleDirections.Count == 0) break;
            nextDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];
            Vector2 proposedCoordinates = actingCoordinates + nextDirection;

            // Try loop again if spot is already taken
            if(CoordinatesAreTaken(proposedCoordinates))
            {
                possibleDirections.Remove(nextDirection);
                continue;
            }

            // Otherwise, if spot is available...
            actingRoom.roomConnections.Add(nextDirection);
            minimapHallLocations.Add(actingCoordinates);
            actingCoordinates = proposedCoordinates;
            minimapHallLocations.Add(actingCoordinates);
            actingRoom = new RoomData();
            if(roomsLeftToFill == 1)
            {
                if(isBranch) actingRoom.roomType = RoomType.Treasure;
                else actingRoom.roomType = RoomType.Boss;
            }
            else actingRoom.roomType = RoomType.Normal;
            actingRoom.roomConnections.Add(-nextDirection);
            roomMap.Add(actingCoordinates, actingRoom);
            minimapRoomLocations.Add(actingCoordinates);
            
            // Branches
            if(!isBranch && Random.value <= 0.3)
            {
                GenerateRoomPath(actingCoordinates, actingRoom, true);
            }

            // Cleanup
            roomsLeftToFill--;
            possibleDirections = cardinalDirections;

            // Overflow Failsafe
            i++;
            if(i > 1000) break;
        }
    }

    private void InstantiateRooms()
    {
        foreach(KeyValuePair<Vector2, RoomData> entry in roomMap)
        {
            Vector2 roomCoordinates = entry.Key;
            RoomData room = entry.Value;
            Vector2 spawnOffset = roomCoordinates * roomSpacing;
            Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
            room.roomInstance = Instantiate(roomPrefab, spawnPosition, transform.rotation);
            DungeonRoom roomScript = room.roomInstance.GetComponent<DungeonRoom>();
            roomScript.SetRoomConnections(room.roomConnections);
            roomScript.SetRoomType((int)room.roomType);
            roomScript.SetRoomCoords(roomCoordinates);
        }
    }

    private void PushMinimapData()
    {
        Minimap.Instance.SetData(minimapRoomLocations, minimapHallLocations);
    }
}

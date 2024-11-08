using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    int mapWidth = 9;
    [SerializeField]
    int mapHeight = 9;
    int[,] map;
    [SerializeField]
    int minimumRooms = 15;

    List<List<Vector2Int>> guides = new List<List<Vector2Int>>();
    

    Vector3 spacing;

    // Start is called before the first frame update
    void Start()
    {
        spacing.x = roomPrefab.GetComponent<SpriteRenderer>().size.x;
        spacing.y = roomPrefab.GetComponent<SpriteRenderer>().size.y;

        map = new int[mapWidth, mapHeight];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PurgeMap();
            GenerateRoomPath(new Vector2Int(mapWidth / 2, mapHeight / 2));
            InstantiateRooms();
        }
    }

    private bool IndexIsWithinMapBounds(Vector2Int mapIndex)
    {
        if(mapIndex.x > 0 && mapIndex.y > 0 &&
            mapIndex.x < mapWidth && mapIndex.y < mapHeight) return true;
        else return false;
    }

    private bool IndexIsEmpty(Vector2Int mapIndex)
    {
        if(map[mapIndex.x, mapIndex.y] == 0) return true;
        else return false;
    }

    private void MarkMapAtIndex(Vector2Int mapIndex)
    {
        map[mapIndex.x, mapIndex.y] = 1;
    }

    private void PurgeMap()
    {
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                map[j, i] = 0;
            }
        }

        guides = new List<List<Vector2Int>>();
    }

    private void GenerateRoomPath(Vector2Int basis)
    {
        Vector2Int nextDirection = new Vector2Int();
        MarkMapAtIndex(basis);
        int roomsLeftToFill = minimumRooms - 1;
        
        // Guide stuff
        List<Vector2Int> guide = new List<Vector2Int>();
        guides.Add(guide);
        guide.Add(basis);


        List<Vector2Int> cardinalDirections = new List<Vector2Int>() {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        List<Vector2Int> possibleDirections = cardinalDirections;

        int i = 0;
        while(roomsLeftToFill > 0)
        {
            if(possibleDirections.Count == 0) break;
            nextDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];
            Vector2Int nextLocation = basis + nextDirection;

            if(IndexIsWithinMapBounds(nextLocation) && IndexIsEmpty(nextLocation))
            {
                basis = nextLocation;
                MarkMapAtIndex(basis);

                // Mark this on our guide, too
                guide.Add(basis);

                // Branches
                if(Random.value <= 0.3)
                {
                    GenerateRoomBranch(basis);
                }

                // Cleanup
                roomsLeftToFill--;
                possibleDirections = cardinalDirections;
            }
            else
            {
                possibleDirections.Remove(nextDirection);
            }
            
            i++;
            if(i > 1000) break;
        }
    }

    private void GenerateRoomBranch(Vector2Int basis)
    {
        Vector2Int nextDirection = new Vector2Int();
        int roomsLeftToFill = Random.Range(1, 4);

        // Guide stuff
        List<Vector2Int> guide = new List<Vector2Int>();
        guides.Add(guide);
        guide.Add(basis);


        int i = 0;
        while(roomsLeftToFill > 0)
        {
            switch(Random.Range(1, 5))
            {
                default:
                case 1:
                    nextDirection = Vector2Int.up;
                    break;
                case 2:
                    nextDirection = Vector2Int.down;
                    break;
                case 3:
                    nextDirection = Vector2Int.left;
                    break;
                case 4:
                    nextDirection = Vector2Int.right;
                    break;
            }

            Vector2Int nextLocation = basis + nextDirection;

            if(IndexIsWithinMapBounds(nextLocation) && IndexIsEmpty(nextLocation))
            {
                basis = nextLocation;
                MarkMapAtIndex(basis);
                
                // Guide stuff
                guide.Add(basis);

                roomsLeftToFill--;
            }

            i++;
            if(i > 1000) break;
        }
    }

    private void InstantiateRooms()
    {
        // Destroy all current rooms
        foreach(Transform child in transform) Destroy(child.gameObject);
        // Remake rooms from map
        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                if(map[j, i] == 1)
                {
                    Vector3 spacingOffset = new Vector3(spacing.x * j, spacing.y * i, 0);
                    Vector3 mapOffset = new Vector3(mapWidth * spacing.x / 2, mapHeight * spacing.y / 2, 0);
                    Vector2 spawnPosition = transform.position + spacingOffset - mapOffset;
                    GameObject roomInstance = Instantiate(roomPrefab, spawnPosition, transform.rotation);
                    roomInstance.transform.SetParent(this.transform);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach(List<Vector2Int> guide in guides)
        {
            Vector2 previousDrawPosition = new Vector2(-1, -1);
            foreach(Vector2Int index in guide)
            {
                Vector3 mapOffset = new Vector3(mapWidth * spacing.x / 2, mapHeight * spacing.y / 2, 0);

                Vector2 drawPosition = transform.position + new Vector3(spacing.x * index.x, spacing.y * index.y, 0) - mapOffset;
                Gizmos.DrawWireSphere(drawPosition, 3);
                if(previousDrawPosition.x != -1)
                {
                    Gizmos.DrawLine(previousDrawPosition, drawPosition);
                }
                previousDrawPosition = drawPosition;
            }
        }
    }
}

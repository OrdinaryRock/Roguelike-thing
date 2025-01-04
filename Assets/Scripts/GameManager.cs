using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public Vector2 roomSpacing;

    [SerializeField]
    private float transitionSpeed = 5f;
    private float transitionTimeElapsed = 0f;
    private Vector3 transitionStartPosition;
    private Vector3 transitionEndPosition;
    private bool transitionComplete = true;

    private Dictionary<Vector2, DungeonGeneratorMK2.RoomData> roomMap;
    private Vector2 activeCoordinates = Vector2.zero;



    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        transitionStartPosition = transform.position;
        transitionEndPosition = transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        transitionTimeElapsed += transitionSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transitionStartPosition, transitionEndPosition, transitionTimeElapsed);
        if(transform.position.Equals(transitionEndPosition) && !transitionComplete)
        {
            transitionComplete = true;
            HeroController.Instance.gameObject.SetActive(true);
            HeroController.Instance.transform.position = (Vector2)transform.position;
            roomMap[activeCoordinates].roomInstance.GetComponent<DungeonRoom>().SetSpawnersActive(true);
            roomMap[activeCoordinates].roomInstance.GetComponent<DungeonRoom>().CloseAllDoors();
        }
    }
    

    public void TransitionRoom(Vector2 direction)
    {
        HeroController.Instance.gameObject.SetActive(false);
        roomMap[activeCoordinates].roomInstance.GetComponent<DungeonRoom>().SetSpawnersActive(false);
        activeCoordinates += direction;
        transitionStartPosition = transform.position;
        transitionEndPosition = transform.position + (Vector3)(direction * roomSpacing);
        transitionTimeElapsed = 0f;
        transitionComplete = false;

        foreach(EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID))
        {
            Destroy(enemy.gameObject);
        }
    }

    public void PushRoomMap(Dictionary<Vector2, DungeonGeneratorMK2.RoomData> map)
    {
        roomMap = map;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D enemyPrefab;
    [SerializeField]
    private Vector2 size = new Vector3(2, 2);
    private Vector3 sizeInV3 = new Vector3();
    [SerializeField]
    private float initialSpawnDelay = 3f;
    [SerializeField]
    private float spawnInterval = 2f;

    private int enemyAmount = 5;

    // Start is called before the first frame update
    private void OnEnable()
    {
        InvokeRepeating(nameof(SpawnEnemy), initialSpawnDelay, spawnInterval);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnEnemy));
    }

    private void SpawnEnemy()
    {
        if(enemyAmount > 0)
        {
            Vector2 spawnPosition = (Vector2) transform.position + new Vector2(Random.Range(size.x / 2, -size.x / 2), Random.Range(size.y / 2, -size.y / 2));
            Rigidbody2D enemyInstance = Instantiate(enemyPrefab, spawnPosition, transform.rotation);
            enemyAmount--;
        }
        else
        {
            CancelInvoke(nameof(SpawnEnemy));
            transform.parent.GetComponent<DungeonRoom>().OpenAllDoors();
        }
    }

    private void OnDrawGizmos()
    {
        sizeInV3.x = size.x;
        sizeInV3.y = size.y;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, sizeInV3);
    }
}

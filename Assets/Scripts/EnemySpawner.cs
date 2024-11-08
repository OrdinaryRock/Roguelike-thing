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

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), initialSpawnDelay, spawnInterval);
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(2, 8), Random.Range(-4, 4));
        Rigidbody2D enemyInstance = Instantiate(enemyPrefab, spawnPosition, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        sizeInV3.x = size.x;
        sizeInV3.y = size.y;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, sizeInV3);
    }
}

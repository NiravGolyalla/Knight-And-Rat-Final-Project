using UnityEngine;

public class TriggeredTileEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1f; // The interval between enemy spawns in seconds

    private bool hasEnteredTrigger = false;
    private float timeSinceLastSpawn = 0f;

    private void Update()
    {
        if (hasEnteredTrigger)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnEnemy(); // Spawn the enemy
                timeSinceLastSpawn = 0f;
            }
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); // Spawn the enemy at the designated point
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knight") || other.CompareTag("Rat"))
        {
            hasEnteredTrigger = true; // Set the flag to true when the player enters the trigger
        }
    }
}

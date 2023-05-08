using System.Collections;
using UnityEngine;

public class FallingBarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fallingBarrelPrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnInterval = 2f;
    private Coroutine spawnBarrelsCoroutine;

    public void StartSpawningBarrels()
    {
        spawnBarrelsCoroutine = StartCoroutine(SpawnBarrels());
    }

    private IEnumerator SpawnBarrels()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(fallingBarrelPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawningBarrels()
    {
        if (spawnBarrelsCoroutine != null)
        {
            StopCoroutine(spawnBarrelsCoroutine);
            spawnBarrelsCoroutine = null;
        }
    }
    
}
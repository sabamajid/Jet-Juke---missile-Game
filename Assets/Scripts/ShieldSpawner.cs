using System.Collections;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab;  // The shield power-up prefab to spawn
    public float spawnRadius = 10f;  // The radius around the player where the shield can spawn
    public float minSpawnTime = 3f; // Minimum time before the next spawn
    public float maxSpawnTime = 5f; // Maximum time before the next spawn

    private GameObject player;

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player");

        // Start spawning shields at random intervals
        StartCoroutine(SpawnShield());
    }

    IEnumerator SpawnShield()
    {
        while (true)
        {
            // Wait for a random time between min and max
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            if (player != null)
            {
                // Get a random position around the player within the spawn radius
                Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.z = 0f;  // Keep it on the 2D plane

                // Instantiate the shield prefab at the spawn position
                Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}

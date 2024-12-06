using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab;  // The shield power-up prefab to spawn
    public float spawnRadius = 10f;  // The radius around the player where the shield can spawn
    public float minSpawnTime = 3f; // Minimum time before the next spawn
    public float maxSpawnTime = 5f; // Maximum time before the next spawn
    public int shieldsToSpawnAtOnce = 1;  // Number of shields to spawn at a time

    private GameObject player;
    private List<GameObject> currentShields = new List<GameObject>();  // List to track currently spawned shields

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
                // Destroy all existing shields before spawning new ones
                foreach (GameObject shield in currentShields)
                {
                    Destroy(shield);
                }
                currentShields.Clear();

                // Spawn the specified number of shields
                for (int i = 0; i < shieldsToSpawnAtOnce; i++)
                {
                    // Get a random position around the player within the spawn radius
                    Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                    spawnPosition.z = 0f;  // Keep it on the 2D plane

                    // Instantiate the shield at the spawn position and add it to the list
                    GameObject newShield = Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
                    currentShields.Add(newShield);
                }
            }
        }
    }
}

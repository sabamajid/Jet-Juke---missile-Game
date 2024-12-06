using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject CoinPrefab;  // The coin prefab to spawn
    public float spawnRadius = 10f;  // The radius around the player where the coins can spawn
    public float minSpawnTime = 3f; // Minimum time before the next spawn
    public float maxSpawnTime = 5f; // Maximum time before the next spawn
    public int coinsToSpawnAtOnce = 1;  // Number of coins to spawn at a time

    private GameObject player;
    private List<GameObject> currentCoins = new List<GameObject>();  // List to track currently spawned coins

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player");

        // Start spawning coins at random intervals
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            // Wait for a random time between min and max
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            if (player != null)
            {
                // Destroy all existing coins before spawning new ones
                foreach (GameObject coin in currentCoins)
                {
                    Destroy(coin);
                }
                currentCoins.Clear();

                // Spawn the specified number of coins
                for (int i = 0; i < coinsToSpawnAtOnce; i++)
                {
                    // Get a random position around the player within the spawn radius
                    Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                    spawnPosition.z = 0f;  // Keep it on the 2D plane

                    // Instantiate the coin at the spawn position and add it to the list
                    GameObject newCoin = Instantiate(CoinPrefab, spawnPosition, Quaternion.identity);
                    currentCoins.Add(newCoin);
                }
            }
        }
    }
}

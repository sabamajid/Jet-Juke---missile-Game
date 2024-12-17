using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject CoinPrefab;  // The coin prefab to spawn
    public float outsideSpawnRadius = 10f;  // Minimum distance outside the player radius where coins will spawn
    public float destroyDistance = 50f;     // Distance from the player when coins will be destroyed (for memory saving)
    public int coinsToSpawnAtOnce = 3;      // Number of coins to spawn at once
    public float checkInterval = 5f;        // Time interval between coin spawn checks (in seconds)

    private GameObject player;
    private List<GameObject> currentCoins = new List<GameObject>();  // List to track currently spawned coins

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("plane");

        if (player != null)
        {
            Debug.Log("Player found! Starting coin spawn.");
            StartCoroutine(SpawnCoinsCheck());
        }
        else
        {
            Debug.Log("Player not found! Make sure the player has the correct tag 'plane'.");
        }
    }

    void Update()
    {
        // Check if the player is null
        if (player == null)
        {
            Debug.Log("Player reference lost! Check if the player object is destroyed or missing.");
        }
        else
        {
            // List to hold coins that need to be removed
            List<GameObject> coinsToDestroy = new List<GameObject>();

            // Continuously check if any coin is farther than destroyDistance and destroy it
            foreach (GameObject coin in currentCoins)
            {
                if (coin != null && Vector3.Distance(player.transform.position, coin.transform.position) > destroyDistance)
                {
                    Debug.Log("Destroying coin at: " + coin.transform.position);
                    coinsToDestroy.Add(coin);
                }
            }

            // Remove all coins that need to be destroyed
            foreach (GameObject coin in coinsToDestroy)
            {
                if (coin != null)
                {
                    Destroy(coin);
                    currentCoins.Remove(coin);
                }
            }
        }
    }

    // Coroutine to periodically check and spawn coins
    IEnumerator SpawnCoinsCheck()
    {
        while (true)
        {
            // Wait for the specified interval before spawning coins
            yield return new WaitForSeconds(checkInterval);

            if (player != null)
            {
                Debug.Log("Checking to spawn coins...");

                // Spawn the specified number of coins
                for (int i = 0; i < coinsToSpawnAtOnce; i++)
                {
                    // Get a random spawn position outside the player's radius
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    spawnPosition.z = 0f;  // Keep it on the 2D plane

                    Debug.Log("Spawning coin at position: " + spawnPosition);

                    // Instantiate the coin and add it to the list
                    GameObject newCoin = Instantiate(CoinPrefab, spawnPosition, Quaternion.identity);
                    currentCoins.Add(newCoin);
                }
            }
            else
            {
                Debug.Log("Player object not found during spawn check.");
                yield break;
            }
        }
    }

    // Method to calculate spawn position outside the player radius
    Vector3 GetRandomSpawnPosition()
    {
        // Generate a random direction
        Vector3 spawnDirection = Random.insideUnitSphere.normalized;  // Random direction in 3D space
        float spawnDistance = Random.Range(outsideSpawnRadius, destroyDistance);  // Distance from player

        // Calculate spawn position
        return player.transform.position + spawnDirection * spawnDistance;
    }
}

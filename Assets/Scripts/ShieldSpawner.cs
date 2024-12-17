using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab;  // The shield power-up prefab to spawn
    public float outsideSpawnRadius = 15f; // Minimum distance outside the player radius where the shield will spawn
    public float destroyDistance = 50f; // The distance from the player when the shield will be destroyed (for memory saving)
    public int shieldsToSpawnAtOnce = 3;  // Number of shields to spawn at once
    public float checkInterval = 5f; // Time interval between shield spawn checks (in seconds)

    private GameObject player;
    private List<GameObject> currentShields = new List<GameObject>();  // List to track currently spawned shields

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("plane");

        if (player != null)
        {
            Debug.Log("Player found!");  // Log if player is found
            // Start the spawn check coroutine if player is found
            StartCoroutine(ShieldSpawnCheck());
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
            // List to hold shields that need to be removed
            List<GameObject> shieldsToDestroy = new List<GameObject>();

            // Continuously check if any shield is farther than destroyDistance and destroy it
            foreach (GameObject shield in currentShields)
            {
                if (shield != null && Vector3.Distance(player.transform.position, shield.transform.position) > destroyDistance)
                {
                    Debug.Log("Destroying shield at: " + shield.transform.position);
                    shieldsToDestroy.Add(shield);  // Mark shield for destruction
                }
            }

            // Remove all shields that need to be destroyed after the loop
            foreach (GameObject shield in shieldsToDestroy)
            {
                if (shield != null)
                {
                    Destroy(shield);  // Destroy the shield
                    currentShields.Remove(shield);  // Remove it from the list
                }
            }
        }
    }

    // Coroutine to periodically check and spawn shields
    IEnumerator ShieldSpawnCheck()
    {
        while (true)
        {
            // Wait for the specified interval before checking and spawning
            yield return new WaitForSeconds(checkInterval);

            if (player != null)
            {
                Debug.Log("Checking to spawn shields...");

                // Spawn the specified number of shields
                for (int i = 0; i < shieldsToSpawnAtOnce; i++)
                {
                    // Generate a random spawn position outside the player's position and the specified radius
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    spawnPosition.z = 0f;  // Keep it on the 2D plane

                    Debug.Log("Spawning shield at position: " + spawnPosition);

                    // Instantiate the shield at the spawn position and add it to the list
                    GameObject newShield = Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
                    currentShields.Add(newShield);
                }
            }
            else
            {
                // If the player is not found, exit the coroutine
                Debug.Log("Player object not found during spawn check.");
                yield break;
            }
        }
    }

    // Method to calculate spawn position outside the player radius
    Vector3 GetRandomSpawnPosition()
    {
        // Generate a random direction from the player
        Vector3 spawnDirection = Random.insideUnitSphere.normalized;  // Random direction in 3D space
        float spawnDistance = Random.Range(outsideSpawnRadius, destroyDistance); // Distance from player (at least outsideSpawnRadius)

        // Calculate the final spawn position using the player's position and the random direction
        return player.transform.position + spawnDirection * spawnDistance;
    }
}

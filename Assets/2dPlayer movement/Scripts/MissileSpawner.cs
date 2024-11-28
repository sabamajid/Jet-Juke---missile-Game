using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missilePrefab;  // The missile prefab
    public Transform planeTransform;  // Reference to the plane's transform
    public float missileSpeed = 5.0f;  // Speed at which the missile travels
    public float missileSpawnRate = 2.0f;  // Time between missile spawns
    public float missileLifetime = 10.0f; // How long each missile stays before being destroyed
    private List<GameObject> activeMissiles = new List<GameObject>(); // List to track active missiles

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2;  // Calculate screen height
        screenWidth = Camera.main.aspect * Camera.main.orthographicSize * 2;  // Calculate screen width
        StartCoroutine(SpawnMissiles());  // Start spawning missiles
    }

    private IEnumerator SpawnMissiles()
    {
        while (true)
        {
            // Only spawn a missile if there are less than 3 active missiles
            if (activeMissiles.Count < 3)
            {
                // Randomly choose from where to spawn the missile (left, right, or bottom)
                int side = Random.Range(0, 3); // 0 = left, 1 = right, 2 = bottom
                Vector3 spawnPosition = Vector3.zero;

                switch (side)
                {
                    case 0:  // Spawn from left side
                        spawnPosition = new Vector3(-screenWidth / 2 - 1, Random.Range(-screenHeight / 2, screenHeight / 2), 0);  // Left edge
                        break;
                    case 1:  // Spawn from right side
                        spawnPosition = new Vector3(screenWidth / 2 + 1, Random.Range(-screenHeight / 2, screenHeight / 2), 0);  // Right edge
                        break;
                    case 2:  // Spawn from bottom side
                        spawnPosition = new Vector3(Random.Range(-screenWidth / 2, screenWidth / 2), -screenHeight / 2 - 1, 0);  // Bottom edge
                        break;
                }

                // Instantiate missile at the spawn position
                GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

                // Add the missile to the active missiles list
                activeMissiles.Add(missile);

                // Start a coroutine to make the missile follow the plane
                StartCoroutine(MissileFollowPlane(missile));

                // Destroy the missile after a set lifetime and remove it from the active list
                Destroy(missile, missileLifetime);

                // Wait for the next missile spawn
                float randomDelay = Random.Range(1f, missileSpawnRate);
                yield return new WaitForSeconds(randomDelay);
            }
            else
            {
                // If there are 3 missiles active, wait before trying to spawn another one
                yield return null;
            }
        }
    }

    private IEnumerator MissileFollowPlane(GameObject missile)
    {
        while (missile != null)
        {
            // Get the direction to the plane
            Vector3 direction = (planeTransform.position - missile.transform.position).normalized;

            // Move the missile toward the plane
            missile.GetComponent<Rigidbody2D>().velocity = direction * missileSpeed;

            // Rotate missile to face the plane
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));  // Add 180 degrees to flip the sprite

            // Wait for the next frame before updating again
            yield return null;
        }

        // Remove the missile from the active list when it gets destroyed
        activeMissiles.Remove(missile);
    }
}

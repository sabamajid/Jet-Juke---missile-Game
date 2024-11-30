using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missilePrefab;  // The missile prefab
    public Transform planeTransform;  // Reference to the plane's transform
    public float missileSpeed = 5.0f;  // Speed at which the missile travels
    public float missileSpawnRate = 2.0f;  // Time between missile spawns
    public float missileLifetime = 15.0f;
    public float missileChasingtime = 10f;
    public float startDelay = 3.0f;  // Delay before spawning the first missile
    private List<GameObject> activeMissiles = new List<GameObject>(); // List to track active missiles

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;  // Reference to the main camera
        StartCoroutine(SpawnMissiles());  // Start spawning missiles
    }

    private IEnumerator SpawnMissiles()
    {
        // Wait for the initial delay before starting to spawn missiles
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Only spawn a missile if there are less than 3 active missiles
            if (activeMissiles.Count < 3)
            {
                // Dynamically calculate screen boundaries
                float screenHeight = mainCamera.orthographicSize * 2;
                float screenWidth = mainCamera.aspect * mainCamera.orthographicSize * 2;

                // Get the center of the camera
                Vector3 cameraCenter = mainCamera.transform.position;

                // Randomly choose from where to spawn the missile (left, right, or bottom)
                int side = Random.Range(0, 3); // 0 = left, 1 = right, 2 = bottom
                Vector3 spawnPosition = Vector3.zero;

                switch (side)
                {
                    case 0:  // Spawn from left side
                        spawnPosition = new Vector3(cameraCenter.x - screenWidth / 2 - 1, Random.Range(cameraCenter.y - screenHeight / 2, cameraCenter.y + screenHeight / 2), 0);
                        break;
                    case 1:  // Spawn from right side
                        spawnPosition = new Vector3(cameraCenter.x + screenWidth / 2 + 1, Random.Range(cameraCenter.y - screenHeight / 2, cameraCenter.y + screenHeight / 2), 0);
                        break;
                    case 2:  // Spawn from bottom side
                        spawnPosition = new Vector3(Random.Range(cameraCenter.x - screenWidth / 2, cameraCenter.x + screenWidth / 2), cameraCenter.y - screenHeight / 2 - 1, 0);
                        break;
                }

                // Instantiate missile at the spawn position
                GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

                // Add the missile to the active missiles list
                activeMissiles.Add(missile);

                // Start a coroutine to make the missile follow the plane
                StartCoroutine(MissileFollowPlane(missile));

                // Destroy the missile after the set lifetime (15 seconds)
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
        float followTime = missileChasingtime;
        float lifetime = missileLifetime;

        // Track time remaining for the missile's lifetime
        float timeLeft = lifetime;

        while (missile != null && timeLeft > 0)
        {
            if (followTime > 0)
            {
                // Get the direction to the plane (chase the player)
                Vector3 direction = (planeTransform.position - missile.transform.position).normalized;

                // Move the missile toward the plane
                missile.GetComponent<Rigidbody2D>().velocity = direction * missileSpeed;

                // Rotate missile to face the plane
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));  // Add 180 degrees to flip the sprite
            }
            else
            {
                // After 10 seconds, move away from the plane
                Vector3 awayDirection = (missile.transform.position - planeTransform.position).normalized;

                // Move the missile away from the plane
                missile.GetComponent<Rigidbody2D>().velocity = awayDirection * missileSpeed;

                // Rotate missile to face away from the plane
                float angle = Mathf.Atan2(awayDirection.y, awayDirection.x) * Mathf.Rad2Deg;
                missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));  // Add 180 degrees to flip the sprite
            }

            // Decrease the time left to follow or move away
            timeLeft -= Time.deltaTime;
            if (followTime > 0)
            {
                followTime -= Time.deltaTime; // Decrease follow time
            }

            // Wait for the next frame before updating again
            yield return null;
        }

        // Remove the missile from the active list when it's no longer following the plane or moving away
        activeMissiles.Remove(missile);
    }
}

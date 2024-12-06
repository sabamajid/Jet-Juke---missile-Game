using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missilePrefab;  // The missile prefab
    public Transform planeTransform;  // Reference to the plane's transform
    public float missileSpeed = 5.0f;  // Speed at which the missile travels
    public float missileSpawnRate = 2.0f;  // Time between missile spawns
    public List<GameObject> activeMissiles = new List<GameObject>(); // List to track active missiles

    private Camera mainCamera;
    private float lastSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;  // Reference to the main camera
        lastSpawnTime = Time.time;  // Initialize spawn time
    }

    void Update()
    {
        // Check if there are fewer than 3 missiles in the list
        if (activeMissiles.Count < 3)
        {
            // If no missiles are present, spawn a new one
            if (activeMissiles.Count == 0 || Time.time >= lastSpawnTime + missileSpawnRate)
            {
                SpawnMissile();
                lastSpawnTime = Time.time;  // Update spawn time
            }
        }

        // Clean up any missiles that are destroyed (null)
        for (int i = activeMissiles.Count - 1; i >= 0; i--)
        {
            if (activeMissiles[i] == null)
            {
                activeMissiles.RemoveAt(i);  // Remove destroyed missile from the list
            }
        }
    }

    private void SpawnMissile()
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
        StartCoroutine(MissileFollowAndFade(missile));
    }

    private IEnumerator MissileFollowAndFade(GameObject missile)
    {
        SpriteRenderer spriteRenderer = missile.GetComponent<SpriteRenderer>();
        Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
        float missileLifetime = 10f;  // Set missile lifetime
        float fadeDuration = 1f;

        // While the missile is alive
        while (missileLifetime > 0)
        {
            if (missile != null)
            {
                // Calculate the direction to the plane
                Vector3 directionToPlane = planeTransform.position - missile.transform.position;
                Vector3 direction = directionToPlane.normalized;

                // Move the missile towards the plane
                rb.velocity = direction * missileSpeed;

                // Rotate the missile to face the plane
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                missile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));  // Add 180 degrees to flip the sprite
            }

            missileLifetime -= Time.deltaTime;
            yield return null;
        }

        // Stop the missile's movement once its lifetime is over
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Fade out the missile
        if (missile != null && spriteRenderer != null)
        {
            float fadeTime = 0;
            Color originalColor = spriteRenderer.color;

            while (fadeTime < fadeDuration)
            {
                if (spriteRenderer != null)
                {
                    // Gradually reduce the alpha value (fading effect)
                    float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
                    spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                }

                fadeTime += Time.deltaTime;
                yield return null;
            }
        }

        // Once faded, remove from active list and destroy the missile
        if (missile != null)
        {
            activeMissiles.Remove(missile);  // Remove missile from active list
            Destroy(missile);  // Destroy missile object
        }
    }
}

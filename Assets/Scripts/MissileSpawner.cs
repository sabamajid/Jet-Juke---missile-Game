using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missilePrefab;  // The missile prefab
    public Transform planeTransform;  // Reference to the plane's transform
    public float missileSpeed = 5.0f;  // Speed at which the missile travels
    public float missileSpawnRate = 2.0f;  // Time between missile spawns
    public float missileLifetime = 10.0f;  // Time before missile starts fading
    public float fadeDuration = 1.0f;  // Time it takes to fade out
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
                StartCoroutine(MissileFollowAndFade(missile));

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

    private IEnumerator MissileFollowAndFade(GameObject missile)
    {
        SpriteRenderer spriteRenderer = missile.GetComponent<SpriteRenderer>();
        Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
        float followTime = missileLifetime;

        while (followTime > 0)
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

            followTime -= Time.deltaTime;
            yield return null;
        }

        // Stop the missile's movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Start fading the missile
        float fadeTime = 0;
        Color originalColor = spriteRenderer.color;

        while (fadeTime < fadeDuration)
        {
            if (spriteRenderer != null)
            {
                // Gradually reduce the alpha
                float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }

            fadeTime += Time.deltaTime;
            yield return null;
        }

        // Remove missile from active list and destroy it
        activeMissiles.Remove(missile);
        Destroy(missile);
    }
}

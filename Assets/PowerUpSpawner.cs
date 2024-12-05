using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner instance;

   
    public GameObject starPrefab;           // Star prefab to spawn
    public float spawnRadius = 10f;         // Radius around the player to spawn the star
    public float speedBoostDuration = 5f;   // Duration of the speed boost after collecting the star
    public float speedBoostAmount = 3f;     // Amount to increase the player's speed
    private GameObject player;              // Reference to the player

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player");

        // Spawn the first star after a delay
        InvokeRepeating("SpawnStar", 2f, 5f);  // Spawn a new star every 5 seconds
    }

    void SpawnStar()
    {
        if (player != null)
        {
            // Generate a random position within the spawn radius around the player
            Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.z = 0f;  // Keep the star on the 2D plane
            Instantiate(starPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public IEnumerator ActivateSpeedBoost()
    {
        // Increase the player's speed
        AirplaneJoystickControl.instance.speed += speedBoostAmount;
        Debug.Log("Speed boosted to: " + AirplaneJoystickControl.instance.speed);

        // Wait for the boost duration to end
        yield return new WaitForSeconds(speedBoostDuration);

        // Reset the player's speed to normal
        AirplaneJoystickControl.instance.speed -= speedBoostAmount;
        Debug.Log("Speed reset to normal");
    }
}

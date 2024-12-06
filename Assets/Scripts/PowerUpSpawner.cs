using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner instance;

    public GameObject starPrefab;           // Star prefab to spawn
    public float spawnRadius = 10f;         // Radius around the player to spawn the star
    public float speedBoostDuration = 5f;   // Duration of the speed boost after collecting the star
    public float speedBoostAmount = 3f;     // Amount to increase the player's speed
    public int powerUpsToSpawnAtOnce = 1;   // Number of power-ups (stars) to spawn at once

    private GameObject player;              // Reference to the player
    private List<GameObject> currentPowerUps = new List<GameObject>();  // List to track currently spawned power-ups

    private bool isSpeedBoostActive = false; // Check if speed boost is active
    private float currentSpeedBoostTime = 0f; // Track current speed boost time

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

        // Start spawning power-ups at regular intervals
        InvokeRepeating("SpawnPowerUps", 1f, 3f);  // Spawn a new set of power-ups every 3 seconds
    }

    void Update()
    {
        // If the speed boost is active, update the remaining time
        if (isSpeedBoostActive)
        {
            currentSpeedBoostTime -= Time.deltaTime;

            if (currentSpeedBoostTime <= 0f)
            {
                // If the boost time has expired, reset the speed and disable the boost
                AirplaneJoystickControl.instance.speed -= speedBoostAmount;
                Debug.Log("Speed reset to normal");
                isSpeedBoostActive = false;
            }
        }
    }

    void SpawnPowerUps()
    {
        if (player != null)
        {
            // Destroy all existing power-ups before spawning new ones
            foreach (GameObject powerUp in currentPowerUps)
            {
                Destroy(powerUp);
            }
            currentPowerUps.Clear();

            // Spawn the specified number of power-ups
            for (int i = 0; i < powerUpsToSpawnAtOnce; i++)
            {
                // Generate a random position within the spawn radius around the player
                Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.z = 0f;  // Keep the star on the 2D plane

                // Instantiate the power-up (star) at the spawn position and add it to the list
                GameObject newPowerUp = Instantiate(starPrefab, spawnPosition, Quaternion.identity);
                currentPowerUps.Add(newPowerUp);
            }
        }
    }

    public void CollectPowerUp()
    {
        if (isSpeedBoostActive)
        {
            // If the speed boost is already active, increase the duration
            currentSpeedBoostTime += speedBoostDuration;
            Debug.Log("Speed boost time extended to: " + currentSpeedBoostTime);
        }
        else
        {
            // Activate the speed boost
            isSpeedBoostActive = true;
            currentSpeedBoostTime = speedBoostDuration;
            AirplaneJoystickControl.instance.speed += speedBoostAmount;
            Debug.Log("Speed boosted to: " + AirplaneJoystickControl.instance.speed);
        }
    }
}

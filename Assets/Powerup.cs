using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    //public PowerUpSpawner powerUpSpawner; // Reference to the PowerUpSpawner for speed boost management

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the star (powerup)
        if (other.CompareTag("Player"))
        {
            // Destroy the star (powerup) after collection
            Destroy(gameObject);

            // Activate the speed boost effect
            PowerUpSpawner.instance.StartCoroutine(PowerUpSpawner.instance.ActivateSpeedBoost());
        }
    }
}

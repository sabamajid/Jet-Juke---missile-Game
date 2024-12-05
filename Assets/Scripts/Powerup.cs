using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the star (powerup)
        if (other.CompareTag("Player"))
        {
            // Destroy the star (powerup) after collection
            Destroy(gameObject);

            // Activate the speed boost effect
            PowerUpSpawner.instance.CollectPowerUp();
        }
    }
}

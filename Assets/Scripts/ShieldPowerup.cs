using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the shield power-up
        if (other.CompareTag("Player"))
        {
            // Activate the player's shield
            PlayerShield playerShield = other.GetComponent<PlayerShield>();
            if (playerShield != null)
            {
                playerShield.EnableShield(5f); // Enable shield for 5 seconds
            }

            // Destroy the shield power-up after activation
            Destroy(gameObject);
        }
    }
}

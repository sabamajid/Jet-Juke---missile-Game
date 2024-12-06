using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the star (powerup)
        if (other.CompareTag("Player"))
        {
            // Destroy the star (powerup) after collection
            Destroy(gameObject);
            CoinManager.instance.AddCoins(1);
        }
    }
}

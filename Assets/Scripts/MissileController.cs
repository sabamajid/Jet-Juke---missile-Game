using UnityEngine;

public class MissileController : MonoBehaviour
{
    public GameObject explosionPrefab; // Reference to the explosion prefab

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is another missile
        if (collision.CompareTag("Missile"))
        {
            // Instantiate explosion at the current position of the missile
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy both missiles
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // Destroy the explosion after 0.2 seconds
            Destroy(explosion, 0.2f);
        }
    }
}

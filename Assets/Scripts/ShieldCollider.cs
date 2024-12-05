using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is a missile
        if (other.CompareTag("Missile"))
        {
            // Destroy the missile
            Destroy(other.gameObject);
            Debug.Log("Missile destroyed by shield!");
        }
    }
}

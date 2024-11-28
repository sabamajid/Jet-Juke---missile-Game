using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Debug.Log("Missile hit the plane!");
            Destroy(other.gameObject);  // Destroy missile
            // You can add more logic here, like reducing health or triggering game over
        }
    }
}

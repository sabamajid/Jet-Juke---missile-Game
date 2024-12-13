using UnityEngine;
using System.Collections;

public class ShieldPowerup : MonoBehaviour
{
    [SerializeField] private AudioSource pickupAudio; 
    private Renderer sphereRenderer;  

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();

        if (pickupAudio != null)
        {
            pickupAudio.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("plane"))
        {
            Debug.Log("hiiiiii");
            // Activate the player's shield
            PlayerShield playerShield = other.GetComponent<PlayerShield>();
            Debug.Log("i got the shield " + playerShield.name);
            if (playerShield != null)
            {
                playerShield.EnableShield(5f); // Enable shield for 5 seconds
            }

            if (pickupAudio != null)
            {
                pickupAudio.Play();
            }
            
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            Debug.Log("biii");
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Set the object's material color to transparent immediately
        Color initialColor = sphereRenderer.material.color;
        sphereRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Wait for 3 seconds before destroying the object
        yield return new WaitForSeconds(3f);

        // After 3 seconds, destroy the object
        Destroy(gameObject);
    }
}

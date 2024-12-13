using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    [SerializeField] private AudioSource pickupAudio;  // AudioSource for power-up pickup sound
    private Renderer sphereRenderer;  // Renderer of the object to modify opacity

    void Start()
    {
        // Get the Renderer component
        sphereRenderer = GetComponent<Renderer>();

        // Ensure the AudioSource is stopped initially
        if (pickupAudio != null)
        {
            pickupAudio.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        // Check if the player collided with the power-up
        if (other.CompareTag("plane"))
        {
            // Play the pickup sound if AudioSource is available
            if (pickupAudio != null)
            {
                pickupAudio.Play();
            }
            PowerUpSpawner.instance.CollectPowerUp();

            // Start fading out the object and then destroy it after a delay
            StartCoroutine(FadeOutAndDestroy());
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
        // Activate the speed boost effect
       
    }
}

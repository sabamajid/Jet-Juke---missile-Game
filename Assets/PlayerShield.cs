using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public GameObject shieldVisual;  // Reference to the shield GameObject (not a child of the player)
    public float shieldFollowDistance = 1.5f;  // Distance the shield stays from the player
    private bool isShieldActive = false;

    private Vector3 shieldOffset;  // Offset to keep the shield at a fixed distance from the player

    void Start()
    {
        // Ensure shield is initially inactive
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    void Update()
    {
        // If the shield is active, keep updating its position to follow the player
        if (isShieldActive && shieldVisual != null)
        {
            // Update shield position based on player position + the offset
            shieldVisual.transform.position = transform.position + shieldOffset;
        }
    }

    public void EnableShield(float duration)
    {
        if (!isShieldActive)
        {
            StartCoroutine(ShieldCoroutine(duration));
        }
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        isShieldActive = true;

        // Set the shield's position offset (this ensures it stays fixed relative to the player)
        shieldOffset = new Vector3(0f, shieldFollowDistance, 0f);  // Change the offset as needed

        // Activate the shield visual
        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        // Debug to show that shield is active
        Debug.Log("Shield activated!");

        yield return new WaitForSeconds(duration);

        // Deactivate the shield visual after the duration
        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        isShieldActive = false;
        Debug.Log("Shield deactivated!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAeroplane : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player

    [Header("Joystick Reference")]
    public Joystick joystick; // Reference to the joystick object

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (joystick == null)
        {
            Debug.LogError("Joystick reference not assigned!");
        }
    }

    void Update()
    {
        // Get joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Create movement vector
        movement = new Vector2(horizontal, vertical);

        // Normalize movement only if magnitude is greater than a small threshold
        if (movement.magnitude > 0.1f)
        {
            movement = movement.normalized;
        }
        else
        {
            movement = Vector2.zero; // Stop movement if input is minimal
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;

        // Rotate the player to face the direction of movement
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

}

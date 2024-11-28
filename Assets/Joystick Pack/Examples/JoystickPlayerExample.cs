using UnityEngine;

public class AirplaneJoystickControl : MonoBehaviour
{
    public float speed = 5f;                // Movement speed
    public float rotationSpeed = 5f;       // Rotation speed
    public VariableJoystick variableJoystick; // Reference to the joystick
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer

    private void Update()
    {
        // Get joystick input
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;

        // Create a movement vector
        Vector2 movement = new Vector2(horizontal, vertical);

        // Move the airplane
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Rotate the airplane to face the direction of movement
        if (movement.sqrMagnitude > 0.01f) // Check if there's significant movement
        {
            // Calculate the angle in degrees based on the joystick direction
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;

            // Create a target rotation
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // Subtract 90 to match airplane orientation

            // Smoothly rotate the airplane towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

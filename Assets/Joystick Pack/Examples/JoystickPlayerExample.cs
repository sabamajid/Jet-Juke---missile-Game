using UnityEngine;

public class AirplaneJoystickControl : MonoBehaviour
{
    public float speed = 5f;                // Movement speed
    public float rotationSpeed = 5f;       // Rotation speed
    public VariableJoystick variableJoystick; // Reference to the joystick
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer

    private Vector2 lastDirection;         // Last movement direction
    private bool isJoystickUsed = false;   // Flag to check if joystick was used

    private void Start()
    {
        // Start moving upwards
        lastDirection = Vector2.up;
    }

    private void Update()
    {
        // Get joystick input
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;

        // Check if joystick is being used
        if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f)
        {
            isJoystickUsed = true;
            lastDirection = new Vector2(horizontal, vertical).normalized;
        }

        // Determine the current movement direction
        Vector2 movement = isJoystickUsed ? lastDirection : Vector2.up;

        // Move the airplane
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Rotate the airplane to face the direction of movement
        if (movement.sqrMagnitude > 0.01f) // Check if there's significant movement
        {
            // Calculate the angle in degrees based on the movement direction
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            // Create a target rotation
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90); // Subtract 90 to match airplane orientation

            // Smoothly rotate the airplane towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

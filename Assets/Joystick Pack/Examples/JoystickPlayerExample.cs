using UnityEngine;

public class AirplaneJoystickControl : MonoBehaviour
{
    public float speed = 5f;                // Movement speed
    public float rotationSpeed = 2f;        // Speed at which the plane rotates
    public float tiltAmount = 30f;          // Maximum tilt angle for pseudo-3D effect
    public float bankAmount = 10f;          // Amount of banking (tilt) during turns
    public VariableJoystick variableJoystick; // Reference to the joystick
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer

    private Vector2 lastDirection = Vector2.up;  // Default initial direction
    private float currentRotationZ = 0f;         // Current z-rotation angle

    private void Update()
    {
        // Get joystick input
        float horizontal = variableJoystick.Horizontal;
        float vertical = variableJoystick.Vertical;

        // Calculate the new direction based on joystick input
        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        // Check if joystick input is significant
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            lastDirection = inputDirection; // Update last direction based on input
        }

        // Move the airplane forward in the current direction
        transform.Translate(lastDirection * speed * Time.deltaTime, Space.World);

        // Rotate the airplane to face the direction of movement smoothly
        SmoothRotateAirplane(lastDirection);
    }

    private void SmoothRotateAirplane(Vector2 direction)
    {
        // Calculate the target angle based on the movement direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Offset to align sprite orientation

        // Gradually rotate towards the target angle
        currentRotationZ = Mathf.MoveTowardsAngle(currentRotationZ, targetAngle, rotationSpeed * Time.deltaTime * 100f);

        // Apply rotation to the airplane
        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);

        // Simulate banking effect by tilting on the y-axis based on turn intensity
        float bankTilt = Mathf.Clamp(-(targetAngle - currentRotationZ) * 0.5f, -bankAmount, bankAmount);
        transform.rotation = Quaternion.Euler(bankTilt, 0, currentRotationZ);
    }
}

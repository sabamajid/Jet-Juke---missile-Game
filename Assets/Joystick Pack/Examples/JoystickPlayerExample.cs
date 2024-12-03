using UnityEngine;

public class AirplaneJoystickControl : MonoBehaviour
{
    public float speed = 5f;                // Movement speed
    public float rotationSpeed = 5f;        // Rotation speed
    public float tiltAmount = 30f;          // Maximum tilt angle for pseudo-3D effect
    public float bankAmount = 10f;          // Amount of banking (tilt) during turns
    public VariableJoystick variableJoystick; // Reference to the joystick
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer

    private Vector2 lastDirection;         // Last movement direction
    private bool isJoystickUsed = false;   // Flag to check if joystick was used
    private float currentRotationZ = 0f;   // Current z-rotation angle

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

        // Rotate the airplane to face the direction of movement and apply smooth tilt
        if (movement.sqrMagnitude > 0.01f) // Check if there's significant movement
        {
            SmoothRotateAirplane(movement, horizontal);
        }
    }

    private void SmoothRotateAirplane(Vector2 movement, float horizontalInput)
    {
        // Calculate the target angle in degrees based on the movement direction
        float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards the target angle using Mathf.LerpAngle
        currentRotationZ = Mathf.LerpAngle(currentRotationZ, targetAngle - 90, rotationSpeed * Time.deltaTime);

        // Apply the rotation to the plane
        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);

        // Calculate tilt for bank effect (smooth leaning during turns)
        float bankTilt = -horizontalInput * bankAmount;
        transform.rotation = Quaternion.Euler(bankTilt, 0, currentRotationZ);
    }
}

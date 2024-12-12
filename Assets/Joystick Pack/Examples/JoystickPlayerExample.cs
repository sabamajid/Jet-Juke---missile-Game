using UnityEngine;

public class AirplaneJoystickControl : MonoBehaviour
{
    public static AirplaneJoystickControl instance;

    public float speed = 5f;                // Movement speed
    public float steerSpeed = 200f;         // Speed at which the plane steers
    public VariableJoystick variableJoystick; // Reference to the joystick

    private Rigidbody2D rb;                 // Rigidbody2D component

    void Awake()
    {
        MakeInstance();
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Get joystick input for horizontal and vertical axis
        float horizontalInput = variableJoystick.Horizontal;  // Use the horizontal joystick input
        float verticalInput = variableJoystick.Vertical;  // Use the vertical joystick input

        // Move the airplane forward in the direction the nose is facing (up direction of the sprite)
        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;

        // Steer the airplane based on horizontal joystick input
        float rotationSteer = horizontalInput * steerSpeed * Time.fixedDeltaTime;

        // Apply angular velocity for steering (rotating around the z-axis)
        rb.angularVelocity = -rotationSteer;  // Steering control

        // Optionally, you can also adjust the plane’s rotation here if you need to visualize it
        // transform.Rotate(Vector3.forward * rotationSteer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour
{

    public Camera cam;
    public Transform plane;
    public GameObject brokenPlane, canvas;
    public Transform leftPoint, rightPoint, forwardPoint;
    Rigidbody2D rb;
    public float speed = 5f, rotateSpeed = 50f;
    Vector3 mousePosition;
    bool isGameOver;

    // Reference to your VariableJoystick (assumed to be attached to a UI joystick object)
    public VariableJoystick variableJoystick;
    Vector2 lastMovementDirection = Vector2.right; // Default starting direction

    void Start()
    {
        if (cam == null) cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            MovePlaneDirectly();
        }
    }

 
    void MovePlaneDirectly()
    {
        // Get the joystick input
        float horizontalInput = variableJoystick.Horizontal;
        float verticalInput = variableJoystick.Vertical;

        // Calculate the current movement direction
        Vector2 currentMovementDirection = new Vector2(horizontalInput, verticalInput);

        // If there is joystick input, update the last movement direction
        if (currentMovementDirection != Vector2.zero)
        {
            lastMovementDirection = currentMovementDirection.normalized;
        }

        // Set the velocity based on the last movement direction
        rb.velocity = lastMovementDirection * speed;

        // Rotate the plane to face the movement direction
        if (lastMovementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(lastMovementDirection.y, lastMovementDirection.x) * Mathf.Rad2Deg;
            rb.rotation = angle; // Rotate the Rigidbody2D to face the movement direction
        }
    }


    void rotatePlane()
    {
        // Invert the horizontal joystick input (multiply by -1)
        float horizontalInput = -variableJoystick.Horizontal;  // Inverted input for left/right rotation

        // Debugging: Log the horizontal input to check if it's responding
        Debug.Log("Horizontal Input: " + horizontalInput);

        float angle;
        Vector2 direction = new Vector2(0, 0);

        // Calculate the direction based on inverted joystick input
        if (horizontalInput < 0) direction = (Vector2)leftPoint.position - rb.position;
        if (horizontalInput > 0) direction = (Vector2)rightPoint.position - rb.position;

        direction.Normalize();
        angle = Vector3.Cross(direction, transform.up).z;
        if (horizontalInput != 0) rb.angularVelocity = -rotateSpeed * angle;
        else rb.angularVelocity = 0;

        // Rotate the plane towards the forward point
        angle = Mathf.Atan2(forwardPoint.position.y - plane.transform.position.y, forwardPoint.position.x - plane.transform.position.x) * Mathf.Rad2Deg;
        plane.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void gameOver(Transform missile)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        isGameOver = true;
        rb.velocity = new Vector2(0, 0);
        cam.gameObject.GetComponent<CameraBehaviour>().gameOver = true;
        GameObject planeTemp = Instantiate(brokenPlane, transform.position, transform.rotation);
        for (int i = 0; i < planeTemp.transform.childCount; i++)
        {
            Rigidbody2D rbTemp = planeTemp.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
            rbTemp.AddForce(((Vector2)missile.position - rbTemp.position) * -5f, ForceMode2D.Impulse);
        }
        StartCoroutine(canvasStuff());
    }

    IEnumerator canvasStuff()
    {
        yield return new WaitForSeconds(0.1f);
        canvas.SetActive(true);
        for (int i = 0; i <= 10; i++)
        {
            float k = (float)i / 10;
            canvas.transform.localScale = new Vector3(k, k, k);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}

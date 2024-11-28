using UnityEngine;

public class SwipeLook : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // Speed of rotation
    private Vector2 touchStart;        // Initial touch/mouse position
    private bool isDragging;           // Is the user dragging?

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        // Detect mouse down
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            isDragging = true;
        }

        // Detect mouse movement
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 touchDelta = (Vector2)Input.mousePosition - touchStart;
            RotateView(touchDelta);
            touchStart = Input.mousePosition; // Update for continuous movement
        }

        // Detect mouse up
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 touchDelta = touch.deltaPosition;
                        RotateView(touchDelta);
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }

    void RotateView(Vector2 delta)
    {
        // Calculate rotation angles
        float yaw = delta.x * rotationSpeed;
        float pitch = -delta.y * rotationSpeed;

        // Apply rotation to the camera
        transform.Rotate(Vector3.up, yaw, Space.World); // Rotate around Y-axis
        transform.Rotate(Vector3.right, pitch, Space.Self); // Rotate around X-axis
    }
}

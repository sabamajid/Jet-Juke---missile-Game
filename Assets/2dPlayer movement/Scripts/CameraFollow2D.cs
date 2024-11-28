using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;  // The player object
    public float smoothSpeed = 0.125f;  // Smoothness of the camera's follow
    public Vector3 offset;  // Offset from the player

    void LateUpdate()
    {
        // Calculate the desired position with the offset
        Vector3 desiredPosition = player.position + offset;

        // Ensure the Z position remains constant
        desiredPosition.z = transform.position.z;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}

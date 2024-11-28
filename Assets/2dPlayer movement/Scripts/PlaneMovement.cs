using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float boundary = 7f;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Restrict player movement within bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        pos.y = Mathf.Clamp(pos.y, -boundary, boundary);
        transform.position = pos;
    }
}

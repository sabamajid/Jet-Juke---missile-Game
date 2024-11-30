using System.Reflection;
using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    public GameObject Blast;
    public GameObject missiles;
    public GameObject GameOverpanel;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Debug.Log("Missile hit the plane!");

            Blast.SetActive(true);

            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color; 
            color.a = 0; 
            spriteRenderer.color = color;

            missiles.SetActive(false);
            GameOverpanel.SetActive(true);
            
        }
    }
}

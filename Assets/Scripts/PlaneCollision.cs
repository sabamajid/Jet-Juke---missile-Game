using UnityEngine;
using System.Collections;

public class PlaneCollision : MonoBehaviour
{
    public GameObject Blast;
    public GameObject missiles;
    public GameObject GameOverpanel;
    public ScoreScript scoreScript; // Reference to ScoreScript
    public GameObject Joystick;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            Debug.Log("Missile hit the plane!");

            // Activate explosion effect
            Blast.SetActive(true);

            // Make the plane invisible
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;

            // Stop the timer and show the score
            if (scoreScript != null)
            {
                TimerScript.instance.StopTimer();  // Access TimerScript's instance to stop the timer
                scoreScript.ShowScores(TimerScript.instance.timeElapsed); // Pass timeElapsed from TimerScript to ScoreScript
            }
            else
            {
                Debug.LogWarning("ScoreScript is not assigned!");
            }

            // Deactivate the missiles
            missiles.SetActive(false);
            Joystick.SetActive(false);

            // Show Game Over panel
            GameOverpanel.SetActive(true);

            gameObject.SetActive(false);

           
        }
    }
}

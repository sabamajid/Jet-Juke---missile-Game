using System.Collections;
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

            // Make the plane invisible
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;

            // Disable missiles
            missiles.SetActive(false);

            // Trigger the Game Over sequence
            StartCoroutine(ShowGameOverPanelWithAnimation());
        }
    }

    private IEnumerator ShowGameOverPanelWithAnimation()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Activate the GameOver panel
        GameOverpanel.SetActive(true);

        // Animate the scale to "pop out" effect
        RectTransform panelTransform = GameOverpanel.GetComponent<RectTransform>();
        Vector3 originalScale = panelTransform.localScale;
        panelTransform.localScale = Vector3.zero; // Start from 0 scale

        float animationDuration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            // Smooth scale animation
            panelTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, Mathf.SmoothStep(0, 1, progress));
            yield return null;
        }

        // Ensure the panel reaches its full size at the end
        panelTransform.localScale = originalScale;
    }
}

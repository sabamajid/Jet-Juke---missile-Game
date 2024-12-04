using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text currentScoreText; // UI Text for current score
    public Text highestScoreText; // UI Text for highest score

    private float currentScore = 0f; // Current calculated score
    private float highestScore = 0f; // Highest score stored in PlayerPrefs

    private void Start()
    {
        // Print a message to confirm Start is called
        Debug.Log("Start method is being called.");

        highestScore = PlayerPrefs.GetFloat("HighestScore");
            // Debugging: Print the stored value
            Debug.Log("Loaded Highest Score from PlayerPrefs: " + highestScore);

            // Update the UI with the loaded score
            highestScoreText.text = "Highest Score: " + Mathf.FloorToInt(highestScore).ToString();
      

        // Show the current score (initially 0)
        UpdateScoreDisplay();
    }



    // This method is used to calculate and update the score
    public void ShowScores(float timeElapsed)
    {
        // Calculate the current score by multiplying the time by 3
        currentScore = timeElapsed * 3;

        // If the current score is greater than the highest score, update it
        if (currentScore > highestScore)
        {
            Debug.Log("I always come here");  // This will log if the condition is true

            highestScore = currentScore;

            // Save the new highest score to PlayerPrefs
            PlayerPrefs.SetFloat("HighestScore", highestScore);
            PlayerPrefs.Save();  // Make sure to save the value

            // Debugging: Check if the highest score is updated
            Debug.Log("Updated Highest Score: " + highestScore);
        }
        else
        {
            // Debugging: If the condition was false, log this
            Debug.Log("Current Score is not higher than the Highest Score.");
        }

        // Update the UI with the current and highest scores
        UpdateScoreDisplay();
    }

    // This method updates the UI to show the current and highest scores
    private void UpdateScoreDisplay()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text = "Current Score: " + Mathf.FloorToInt(currentScore).ToString();
        }

        if (highestScoreText != null)
        {
            highestScoreText.text = "Highest Score: " + Mathf.FloorToInt(highestScore).ToString();
        }
    }
}

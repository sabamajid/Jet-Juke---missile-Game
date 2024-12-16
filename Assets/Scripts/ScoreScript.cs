using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance; // Singleton instance

    public Text currentScoreText; // UI Text for current score
    public Text highestScoreText; // UI Text for highest score
    public Text lastTimeText; // UI Text for last time
    public GameObject gameOverPanel; // Reference to the Game Over panel
    public Text CoinWithBonus; // UI Text to show coins with bonus

    private float currentScore = 0f; // Current calculated score
    private float highestScore = 0f; // Highest score stored in PlayerPrefs

    private void Awake()
    {
        // Ensure that the instance is set
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        // Load the highest score from PlayerPrefs
        highestScore = PlayerPrefs.GetFloat("HighestScore", 0f);

        // Initialize the highest score text
        if (highestScoreText != null)
        {
            highestScoreText.text = "" + Mathf.FloorToInt(highestScore).ToString();
        }

        // Game Over panel is inactive initially; do not update score immediately
        if (gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            Debug.Log("Game Over panel is inactive. Waiting to update scores.");
        }
    }

    public void UpdateScores(float timeElapsed, float points)
    {
        // Update the current score
        currentScore = points;

        // Update the highest score if necessary
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetFloat("HighestScore", highestScore);
            PlayerPrefs.Save();
        }

        // Wait to update the UI until the Game Over panel is active
        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            UpdateScoreDisplay(timeElapsed);
        }
        else
        {
            Debug.Log("Game Over panel is not active. Deferring score update.");
        }
    }

    public void OnGameOverPanelActivated()
    {
        // Update scores when the Game Over panel is activated
        UpdateScoreDisplay(TimerScript.instance.timeElapsed);
    }

    private void UpdateScoreDisplay(float timeElapsed)
    {
        // Update the current score display
        if (currentScoreText != null)
        {
            currentScoreText.text = "" + Mathf.FloorToInt(currentScore).ToString();
        }

        // Update the highest score display
        if (highestScoreText != null)
        {
            highestScoreText.text = "" + Mathf.FloorToInt(highestScore).ToString();
        }

        // Update the last time display
        if (lastTimeText != null)
        {
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            lastTimeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        // Calculate and add bonus coins
        AddBonusCoins(timeElapsed);
    }
    private void AddBonusCoins(float timeElapsed)
    {
        // Calculate bonus coins based on time elapsed
        int bonusCoins = Mathf.FloorToInt(timeElapsed / 25); // 25 seconds = 1 coin

        // Add bonus coins to the CoinManager (affecting both current and persistent totals)
        CoinManager.instance.AddCoins(bonusCoins);

        // Get the current total coins from CoinManager (after adding bonus coins)
        int totalCoins = CoinManager.instance.coinCount; // Current session coins from CoinManager

        // Update the CoinWithBonus text to show the total coins only (bonus coins added)
        if (CoinWithBonus != null)
        {
            CoinWithBonus.text = "" + (totalCoins + bonusCoins);
        }

        Debug.Log($"Total coins after bonus: {totalCoins}");
    }



}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance; // Singleton instance

    public Text currentScoreText; // UI Text for current score
    public Text highestScoreText; // UI Text for highest score
    public Text lastTimeText; // UI Text for last time
    public GameObject gameOverPanel; // Main Game Over canvas
    public Text CoinWithBonus; // UI Text to show coins with bonus

    private float currentScore = 0f; // Current calculated score
    private float highestScore = 0f; // Highest score stored in PlayerPrefs
    public GameObject NewHightestScorePanel; // Panel for new highest score
    public GameObject GameOverPael; // Panel for normal game over

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the highest score from PlayerPrefs
        highestScore = PlayerPrefs.GetFloat("HighestScore", 0f);

        // Initialize the highest score text
        if (highestScoreText != null)
        {
            highestScoreText.text = Mathf.FloorToInt(highestScore).ToString();
        }
    }

    public void UpdateScores(float timeElapsed, float points)
    {
        // Update the current score
        currentScore = points;

        if (currentScore > highestScore)
        {
            // Save new highest score
            highestScore = currentScore;
            PlayerPrefs.SetFloat("HighestScore", highestScore);
            PlayerPrefs.Save();

            // Show the highest score panel
            StartCoroutine(ShowHighestScorePanel());
        }
        else
        {
            // Directly update the normal game over panel
            UpdateScoreDisplay(timeElapsed);
        }
    }

    private IEnumerator ShowHighestScorePanel()
    {
        // Show the highest score panel
        if (GameOverPael != null && NewHightestScorePanel != null)
        {
            GameOverPael.SetActive(false);
            NewHightestScorePanel.SetActive(true);

            // Wait for 2 seconds
            yield return new WaitForSeconds(2f);

            // Revert back to the normal game over panel
            NewHightestScorePanel.SetActive(false);
            GameOverPael.SetActive(true);
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
            currentScoreText.text = Mathf.FloorToInt(currentScore).ToString();
        }

        // Update the highest score display
        if (highestScoreText != null)
        {
            highestScoreText.text = Mathf.FloorToInt(highestScore).ToString();
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

        // Add bonus coins to the CoinManager
        CoinManager.instance.AddCoins(bonusCoins);

        // Get the updated total coins
        int totalCoins = CoinManager.instance.coinCount;

        // Update the CoinWithBonus text
        if (CoinWithBonus != null)
        {
            CoinWithBonus.text = totalCoins.ToString();
        }

        Debug.Log($"Total coins after bonus: {totalCoins}");
    }
}

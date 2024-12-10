using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false; // Tracks if the game is paused
    public GameObject PausePanel;


    // Function to toggle the pause state
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Function to pause the game
    public void PauseGame()
    {
        Time.timeScale = 0; // Freeze the game
        isPaused = true;
        PausePanel.SetActive(true);
    }

    // Function to resume the game
    public void ResumeGame()
    {
        Time.timeScale = 1; // Unfreeze the game
        isPaused = false;
        PausePanel.SetActive(false);
    }

    // Optional: Pausing through Escape key (for PC)
    private void Update()
    {
       
    }
}

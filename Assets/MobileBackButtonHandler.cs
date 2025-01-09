using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileBackButtonHandler : MonoBehaviour
{
    private void Update()
    {
        // Check if the back button (Escape key) is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the game
            ExitGame();
        }
    }

    private void ExitGame()
    {
       
        // Check if the current scene is a specific one
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        // Example: Perform an action when in a specific scene
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


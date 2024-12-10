using UnityEngine;
using UnityEngine.UI; // To reference UI buttons

public class GameAudioManager : MonoBehaviour
{
    private bool isMuted;
    public Button muteButton; // Reference to the mute button
    public Button unmuteButton; // Reference to the unmute button

    // Start is called before the first frame update
    void Start()
    {
        // Load the mute state from PlayerPrefs
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        // Set the initial volume based on the loaded state
        AudioListener.volume = isMuted ? 0 : 1;

        // Update button states based on mute status
        UpdateButtonStates();
    }

    // Toggle mute
    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle the mute state
        AudioListener.volume = isMuted ? 0 : 1;

        // Save the mute state to PlayerPrefs
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save(); // Ensure the changes are saved immediately

        // Update button states after toggling
        UpdateButtonStates();
    }

    // Unmute manually
    public void ToggleUnmute()
    {
        isMuted = false; // Set isMuted to false to unmute
        AudioListener.volume = 1;

        // Save the unmute state to PlayerPrefs
        PlayerPrefs.SetInt("Muted", 0);
        PlayerPrefs.Save();

        // Update button states after unmuting
        UpdateButtonStates();
    }
    private void UpdateButtonStates()
    {
        // Show the mute button if not muted, and hide it if muted
        muteButton.gameObject.SetActive(!isMuted);

        // Show the unmute button if muted, and hide it if not muted
        unmuteButton.gameObject.SetActive(isMuted);
    }

}

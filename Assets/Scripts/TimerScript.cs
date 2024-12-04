using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static TimerScript instance; // Singleton instance

    public bool isTimerRunning = false;
    public float timeElapsed = 0f;
    public UnityEngine.UI.Text timerText;

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

        DontDestroyOnLoad(gameObject); // Optional: if you want this script to persist across scenes
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}

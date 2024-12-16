using UnityEngine;
using UnityEngine.UI; // For displaying the coin count in UI

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;  // Current coin count
    public int totalCoins = 0; // Total coins (persistent)

    // Arrays of UI Text elements to display the coin counts
    public Text[] temporaryCoinTexts;  // Temporary coin texts (e.g., current session)
    public Text[] totalCoinTexts;      // Total coin texts (persistent)

    private const string CoinKey = "Coins";  // Key to store the total coin data in PlayerPrefs

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    { 
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Function to add coins
    public void AddCoins(int amount)
    {
        coinCount += amount;
        totalCoins += amount;  // Add to total coins as well
        SaveCoins();
        UpdateCoinDisplay();
    }

    // Update the coin count display across the UI Text elements
    private void UpdateCoinDisplay()
    {
        // Update temporary coin texts (e.g., for current session)
        foreach (Text coinText in temporaryCoinTexts)
        {
            if (coinText != null)
            {
                coinText.text = "" + coinCount.ToString();
            }
        }

        // Update total coin texts (persistent total coins)
        foreach (Text coinText in totalCoinTexts)
        {
            if (coinText != null)
            {
                coinText.text = "" + totalCoins.ToString();
            }
        }
    }

    // Reset the temporary coins (not affecting total coins)
    public void ResetTemporaryCoins()
    {
        coinCount = 0;
        UpdateCoinDisplay();
    }

    // Save the current total coin count to PlayerPrefs
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, totalCoins);
        PlayerPrefs.Save();  // Ensure the data is saved immediately
    }

    // Load the total coin count from PlayerPrefs
    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt(CoinKey, 0);  // Default to 0 if no value is found
        UpdateCoinDisplay();
    }

    // Called when the game starts
    private void Start()
    {
        LoadCoins();  // Load the saved total coin count
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfig config;
    [SerializeField] private GameObject losePanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI finalCoinsText;

    public float ScrollSpeed { get; private set; }
    public float Distance { get; private set; }
    public int Coins { get; private set; }
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;
        IsGameOver = false;
        Distance = 0f;
        Coins = 0;

        if (config != null)
            ScrollSpeed = config.startSpeed;

        if (losePanel != null)
            losePanel.SetActive(false);

        UpdateDistanceUI();
        UpdateCoinsUI();
    }

    void Update()
    {
        if (IsGameOver) return;
        if (config == null) return;

        ScrollSpeed = Mathf.Min(
            ScrollSpeed + config.speedIncreaseRate * Time.deltaTime,
            config.maxSpeed
        );

        Distance += ScrollSpeed * Time.deltaTime;

        UpdateDistanceUI();
    }

    public void AddCoin(int amount)
    {
        Coins += amount;
        UpdateCoinsUI();
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        ScrollSpeed = 0f;

        UpdateFinalUI();

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    private void UpdateDistanceUI()
    {
        if (distanceText == null) return;

        int distance = Mathf.FloorToInt(Distance);
        distanceText.text = distance + " m";
    }

    private void UpdateCoinsUI()
    {
        if (coinsText == null) return;

        coinsText.text = Coins.ToString();
    }

    private void UpdateFinalUI()
    {
        int finalDistance = Mathf.FloorToInt(Distance);

        if (finalDistanceText != null)
            finalDistanceText.text = "Distance: " + finalDistance + " m";

        if (finalCoinsText != null)
            finalCoinsText.text = "Coins: " + Coins;
    }

    public void ReplayGame()
    {
        if (losePanel != null)
            losePanel.SetActive(false);

        IsGameOver = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu()
    {
        if (losePanel != null)
            losePanel.SetActive(false);

        IsGameOver = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
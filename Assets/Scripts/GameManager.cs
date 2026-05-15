using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string HighScoreKey = "HighScore";

    [SerializeField] private GameConfig config;

    [Header("Panels")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI finalCoinsText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI finalHighScoreText;

    public float ScrollSpeed { get; private set; }
    public float Distance { get; private set; }
    public int Coins { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }

    void Awake()
    {
        Instance = this;

        Time.timeScale = 1f;
        IsGameOver = false;
        IsPaused = false;
        Distance = 0f;
        Coins = 0;

        if (config != null)
            ScrollSpeed = config.startSpeed;

        if (losePanel != null)
            losePanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        UpdateDistanceUI();
        UpdateCoinsUI();
        UpdateHighScoreUI();
    }

    void Update()
    {
        if (IsGameOver || IsPaused) return;
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
        if (IsGameOver || IsPaused) return;

        Coins += amount;
        UpdateCoinsUI();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayCoinPickup();
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        IsPaused = false;
        ScrollSpeed = 0f;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        CheckHighScore();
        UpdateFinalUI();
        UpdateHighScoreUI();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayGameLose();

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    private void CheckHighScore()
    {
        int finalDistance = Mathf.FloorToInt(Distance);
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (finalDistance > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, finalDistance);
            PlayerPrefs.Save();
        }
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

    private void UpdateHighScoreUI()
    {
        if (highScoreText == null) return;

        if (!PlayerPrefs.HasKey(HighScoreKey))
        {
            highScoreText.gameObject.SetActive(false);
            return;
        }

        highScoreText.gameObject.SetActive(true);
        highScoreText.text = "Best: " + PlayerPrefs.GetInt(HighScoreKey) + " m";
    }

    private void UpdateFinalUI()
    {
        int finalDistance = Mathf.FloorToInt(Distance);
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (finalDistanceText != null)
            finalDistanceText.text = "d = " + finalDistance + " m";

        if (finalCoinsText != null)
            finalCoinsText.text = Coins.ToString();

        if (finalHighScoreText != null)
            finalHighScoreText.text = "Highest Score: " + highScore + " m";
    }

    public void PauseGame()
    {
        PlayButtonSound();

        if (IsGameOver) return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        PlayButtonSound();

        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void ReplayGame()
    {
        PlayButtonSound();

        if (losePanel != null)
            losePanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        IsGameOver = false;
        IsPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main");
    }

    public void GoToMainMenu()
    {
        PlayButtonSound();

        if (losePanel != null)
            losePanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        IsGameOver = false;
        IsPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    private void PlayButtonSound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }
}
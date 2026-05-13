using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfig config;
    [SerializeField] private GameObject losePanel;

    public float ScrollSpeed { get; private set; }
    public float Distance { get; private set; }
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (config != null)
            ScrollSpeed = config.startSpeed;

        if (losePanel != null)
            losePanel.SetActive(false);
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
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        ScrollSpeed = 0f;

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI scoreText;

    [Header("Game Settings")]
    public int winScore = 200;

    [Header("Player Settings")]
    public GameObject player;

    private int currentScore = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    void OnEnable() { EnergyCore.OnCoreDestroyed += HandleGameOver; }
    void OnDisable() { EnergyCore.OnCoreDestroyed -= HandleGameOver; }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        UpdateScoreUI();

        // KAZANMA KONTROLÜ
        if (currentScore >= winScore)
        {
            HandleGameWin();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + currentScore;
        }
    }

    private void HandleGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandleGameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("YOU WIN!");
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
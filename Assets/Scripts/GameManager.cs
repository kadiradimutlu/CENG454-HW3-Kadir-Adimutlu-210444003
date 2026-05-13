using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action<int> OnScoreChanged;

    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    [Header("Survival Settings")]
    public float requiredSurvivalSeconds = 120f;
    public float coreDamageTimePenaltySeconds = 5f;

    [Header("Player Settings")]
    public GameObject player;

    private int currentScore = 0;
    private float elapsedSurvivalSeconds = 0f;
    private bool isGameOver = false;

    public int CurrentScore => currentScore;
    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        EnergyCore.OnCoreDestroyed += HandleGameOver;
        EnergyCore.OnCoreDamaged += HandleCoreDamaged;
    }

    void OnDisable()
    {
        EnergyCore.OnCoreDestroyed -= HandleGameOver;
        EnergyCore.OnCoreDamaged -= HandleCoreDamaged;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
        OnScoreChanged?.Invoke(currentScore);
    }

    void Update()
    {
        if (isGameOver) return;

        elapsedSurvivalSeconds += Time.deltaTime;
        UpdateTimerUI();

        if (elapsedSurvivalSeconds >= requiredSurvivalSeconds)
        {
            HandleGameWin();
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(currentScore);
    }

    private void HandleCoreDamaged(int damageAmount)
    {
        if (isGameOver) return;

        requiredSurvivalSeconds += coreDamageTimePenaltySeconds;
        UpdateTimerUI();
}

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + currentScore;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        float remainingSeconds = Mathf.Max(0f, requiredSurvivalSeconds - elapsedSurvivalSeconds);
        timerText.text = "SURVIVE: " + Mathf.CeilToInt(remainingSeconds) + "s";
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



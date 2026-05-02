using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel;

    [Header("Player Settings")]
    public GameObject player;

    void OnEnable()
    {
        EnergyCore.OnCoreDestroyed += HandleGameOver;
    }

    void OnDisable()
    {
        EnergyCore.OnCoreDestroyed -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        Debug.Log("GAME OVER! The core has been breached.");
        
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
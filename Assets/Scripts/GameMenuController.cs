using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject hudPanel;

    private bool gameStarted = false;
    private bool isPaused = false;

    private void Awake()
    {
        ResolveReferencesFromScene();

        Time.timeScale = 0f;

        SetMainMenuActive(true);
        SetPauseMenuActive(false);
        SetHudActive(false);
SetOptionalPanelActive("GameOverPanel", false);
SetOptionalPanelActive("WinPanel", false);

UnlockCursor();
    }

    private void Start()
    {
        ResolveReferencesFromScene();
        UnlockCursor();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            UnlockCursor();
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (isPaused)
        {
            UnlockCursor();
        }
    }

    public void StartGame()
    {
        ResolveReferencesFromScene();

        gameStarted = true;
        isPaused = false;

        Time.timeScale = 1f;

        SetMainMenuActive(false);
        SetPauseMenuActive(false);
        SetHudActive(true);
SetOptionalPanelActive("GameOverPanel", false);
SetOptionalPanelActive("WinPanel", false);

LockCursor();

    }

    public void PauseGame()
    {
        if (!gameStarted) return;

        isPaused = true;
        Time.timeScale = 0f;

        SetPauseMenuActive(true);

        UnlockCursor();
    }

    public void ResumeGame()
    {
        if (!gameStarted) return;

        isPaused = false;
        Time.timeScale = 1f;

        SetPauseMenuActive(false);

        LockCursor();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetMainMenuActive(bool active)
    {
        SetPanelByExactName("MainMenuPanel", active);
    }

    private void SetPauseMenuActive(bool active)
    {
        SetPanelByExactName("PauseMenuPanel", active);
    }

    private void SetHudActive(bool active)
    {
        SetPanelByExactName("HUDPanel", active);
    }

    private void ResolveReferencesFromScene()
    {
        mainMenuPanel = FindSceneObjectByExactName("MainMenuPanel");
        pauseMenuPanel = FindSceneObjectByExactName("PauseMenuPanel");
        hudPanel = FindSceneObjectByExactName("HUDPanel");

        if (mainMenuPanel == null)
        {
            Debug.LogError("GameMenuController could not find MainMenuPanel in the scene.");
        }

        if (pauseMenuPanel == null)
        {
            Debug.LogError("GameMenuController could not find PauseMenuPanel in the scene.");
        }

        if (hudPanel == null)
        {
            Debug.LogError("GameMenuController could not find HUDPanel in the scene.");
        }
    }

    private void SetOptionalPanelActive(string objectName, bool active)
    {
        GameObject target = FindSceneObjectByExactName(objectName);

        if (target != null)
        {
            target.SetActive(active);
        }
    }

    private void SetPanelByExactName(string objectName, bool active)
    {
        GameObject target = FindSceneObjectByExactName(objectName);

        if (target == null)
        {
            Debug.LogError("Panel not found: " + objectName);
            return;
        }

        target.SetActive(active);
    }

    private GameObject FindSceneObjectByExactName(string objectName)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            Transform[] allChildren = rootObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in allChildren)
            {
                if (child.name == objectName)
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}



using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverScoreText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Victory UI")]
    public TextMeshProUGUI victoryScoreText;
    public Button victoryRestartButton;
    public Button victoryMainMenuButton;

    [Header("Pause UI")]
    public Button resumeButton;
    public Button pauseMainMenuButton;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ẩn tất cả panels ban đầu
        HideAllPanels();

        // Setup button listeners
        SetupButtonListeners();
    }

    private void SetupButtonListeners()
    {
        // Game Over buttons
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Victory buttons
        if (victoryRestartButton != null)
            victoryRestartButton.onClick.AddListener(RestartGame);
        if (victoryMainMenuButton != null)
            victoryMainMenuButton.onClick.AddListener(GoToMainMenu);

        // Pause buttons
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (pauseMainMenuButton != null)
            pauseMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Update()
    {
        // Pause game with Escape key (for testing)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ShowGameOver()
    {
        HideAllPanels();
        gameOverPanel.SetActive(true);

        // Hiển thị điểm số cuối cùng
        if (gameOverScoreText != null && GameManager.instance != null)
        {
            gameOverScoreText.text = "Score: " + GameManager.instance.score;
        }

        // Pause game
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        Debug.Log("GameUI.ShowVictory() called");
        HideAllPanels();
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Debug.Log("Victory panel activated");
        }
        else
        {
            Debug.LogError("Victory panel is NULL!");
        }

        // Hiển thị điểm số cuối cùng
        if (victoryScoreText != null && GameManager.instance != null)
        {
            victoryScoreText.text = "Final Score: " + GameManager.instance.score;
            Debug.Log($"Victory score text set to: {victoryScoreText.text}");
        }
        else
        {
            Debug.LogError("Victory score text or GameManager is NULL!");
        }

        // Pause game
        Time.timeScale = 0f;
        Debug.Log("Game paused (Time.timeScale = 0)");
    }

    public void PauseGame()
    {
        if (gameOverPanel.activeInHierarchy || victoryPanel.activeInHierarchy)
            return;

        HideAllPanels();
        pausePanel.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        HideAllPanels();
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    private void HideAllPanels()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // Thay đổi tên scene menu chính nếu cần
    }
}



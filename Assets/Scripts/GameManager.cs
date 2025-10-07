using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int lives = 3;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    [Header("Game Settings")]
    public int victoryScore = 5; // Số coin cần để thắng
    public bool gameEnded = false;

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

    void Start()
    {
        score = 0;
        lives = 3;
        gameEnded = false;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    // void UpdateUI()
    // {
    //     if (scoreText != null)
    //         scoreText.SetText(score.ToString());
    //     if (livesText != null)
    //         livesText.SetText(lives.ToString());
    // }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.SetText(score.ToString());
        if (livesText != null)
        {
            // Hiển thị số mạng quiz dạng "Quiz Lives: 3"
            livesText.SetText("Quiz Lives: " + lives);
        }
    }

    public void AddScore(int points)
    {
        if (!gameEnded)
        {
            score += points;
        }
    }

    public void CheckVictory()
    {
        if (!gameEnded && score >= victoryScore)
        {
            gameEnded = true;
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        if (GameUI.instance != null)
        {
            GameUI.instance.ShowVictory();
        }
    }

    public void LoseLife(int amount)
    {
        if (!gameEnded)
        {
            lives -= amount;
            if (lives <= 0)
            {
                lives = 0;
                gameEnded = true;
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        if (GameUI.instance != null)
        {
            GameUI.instance.ShowGameOver();
        }
    }

    public bool IsVictoryActive()
    {
        if (GameUI.instance != null)
        {
            return GameUI.instance.IsVictoryPanelActive();
        }
        return false;
    }

    public void ResetGame()
    {
        score = 0;
        lives = 3;
        gameEnded = false;
        UpdateUI();
    }
}
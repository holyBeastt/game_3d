using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Game Settings")]
    public int victoryScore = 5; // Số coin cần để thắng
    private bool gameEnded = false;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoreText != null)
            scoreText.SetText(score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
            scoreText.SetText(score.ToString());

        // Kiểm tra điều kiện thắng
        if (!gameEnded && score >= victoryScore)
        {
            gameEnded = true;
            ShowVictory();
        }
    }

    public void AddScore(int points)
    {
        if (!gameEnded)
        {
            score += points;
        }
    }

    private void ShowVictory()
    {
        if (GameUI.instance != null)
        {
            GameUI.instance.ShowVictory();
        }
    }

    public void ResetGame()
    {
        score = 0;
        gameEnded = false;
    }
}

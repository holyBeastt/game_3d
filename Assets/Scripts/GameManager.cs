using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Force set victory score to 5
        victoryScore = 5;
        Debug.Log($"Victory score set to: {victoryScore}");
        
        if (scoreText != null)
            scoreText.SetText(score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
            scoreText.SetText(score.ToString());
    }

    public void AddScore(int points)
    {
        Debug.Log($"AddScore called with {points} points. Current score: {score}, gameEnded: {gameEnded}");
        
        if (!gameEnded)
        {
            score += points;
            Debug.Log($"Score updated to: {score}, victoryScore: {victoryScore}");
            
            // Kiểm tra victory ngay sau khi cộng điểm
            if (score >= victoryScore)
            {
                Debug.Log("VICTORY CONDITION MET! Setting gameEnded = true and calling ShowVictory()");
                gameEnded = true;
                ShowVictory();
            }
            else
            {
                Debug.Log($"Not enough points yet. Need {victoryScore - score} more points.");
            }
        }
        else
        {
            Debug.Log("Game already ended, not adding score");
        }
    }

    private void ShowVictory()
    {
        Debug.Log("ShowVictory() called");
        if (GameUI.instance != null)
        {
            Debug.Log("GameUI.instance found, calling ShowVictory()");
            GameUI.instance.ShowVictory();
        }
        else
        {
            Debug.LogError("GameUI.instance is NULL! Victory panel cannot be shown.");
        }
    }

    public void ResetGame()
    {
        score = 0;
        gameEnded = false;
    }
}

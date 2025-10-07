using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    [Header("Gameplay")]
    public int lives = 3;
    private int correctAnswerIndex;

    void Start()
    {
        HideQuiz();
    }

    // Gọi khi nhặt coin (60% xác suất)
    public void TryShowQuiz()
    {
        if (Random.value <= 0.6f) // 60% xác suất
        {
            ShowQuiz();
        }
    }

    void ShowQuiz()
    {
        // Câu hỏi mẫu
        questionText.text = "Câu nào là màu của bầu trời?";
        string[] answers = { "Xanh", "Đỏ", "Vàng", "Đen" };
        correctAnswerIndex = 0; // "Xanh" là đúng

        // Gán text cho các nút
        for (int i = 0; i < answerButtons.Length; i++) // ✅ đã sửa
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
            int index = i; // tránh lỗi capture biến
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }

        // Hiện panel
        quizPanel.alpha = 1;
        quizPanel.interactable = true;
        quizPanel.blocksRaycasts = true;

        // Dừng chuyển động (nếu có Player)
        if (Player.instance != null)
        {
            Player.instance.enabled = false;
        }
    }

    void OnAnswerSelected(int index)
    {
        if (index == correctAnswerIndex)
        {
            Debug.Log("✅ Câu trả lời đúng!");
            HideQuiz();
        }
        else
        {
            lives--;
            Debug.Log($"❌ Sai rồi! Còn lại: {lives} mạng");
            if (lives <= 0)
            {
                var ui = FindObjectOfType<GameUI>();
                // if (ui != null)
                // {
                //     ui.GameOverPanel.SetActive(true);
                // }
            }
            else
            {
                HideQuiz();
            }
        }
    }

    void HideQuiz()
    {
        quizPanel.alpha = 0;
        quizPanel.interactable = false;
        quizPanel.blocksRaycasts = false;

        // Cho player tiếp tục chạy
        if (Player.instance != null)
        {
            Player.instance.enabled = true;
        }
    }
}

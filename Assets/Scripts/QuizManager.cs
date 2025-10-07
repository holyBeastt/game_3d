using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    private int correctAnswerIndex;
    private Coin currentCoin; // Lưu coin đang xử lý

    void Start()
    {
        HideQuiz();
    }

    // Gọi khi nhặt coin
    public void ShowQuizWithCoin(Coin coin)
    {
        // Nếu đang hiện Victory thì không hiện quiz
        if (GameManager.instance != null && GameManager.instance.IsVictoryActive())
            return;

        currentCoin = coin;
        ShowQuiz();
    }

    void ShowQuiz()
    {
        // Câu hỏi mẫu
        questionText.text = "Câu nào là màu của bầu trời?";
        string[] answers = { "Xanh", "Đỏ", "Vàng", "Đen" };
        correctAnswerIndex = 0; // "Xanh" là đúng

        // Gán text cho các nút
        for (int i = 0; i < answerButtons.Length; i++)
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
        if (PlayerExists())
        {
            Player.instance.enabled = false;
        }
    }

    void OnAnswerSelected(int index)
    {
        if (index == correctAnswerIndex)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
                GameManager.instance.CheckVictory();
            }
        }
        else
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife(1);
            }
        }

        // Hủy coin sau khi trả lời
        if (currentCoin != null)
        {
            Destroy(currentCoin.gameObject);
            currentCoin = null;
        }

        HideQuiz();
    }

    void HideQuiz()
    {
        quizPanel.alpha = 0;
        quizPanel.interactable = false;
        quizPanel.blocksRaycasts = false;

        // Cho player tiếp tục chạy
        if (PlayerExists())
        {
            Player.instance.enabled = true;
        }
    }

    // Tránh lỗi nếu không có Player script
    bool PlayerExists()
    {
        return (typeof(Player).IsAssignableFrom(typeof(MonoBehaviour)) && Player.instance != null)
            || (Player.instance != null);
    }
}
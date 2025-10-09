using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers = new string[4];
    public int correctIndex;
}

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text resultText;

    [Header("Quiz Data")]
    public QuizQuestion[] questions;

    private int correctAnswerIndex;
    private Coin currentCoin;
    private Color defaultBtnColor;
    private QuizQuestion currentQuestion;
    private bool isQuizActive = false;
    private int currentQuizIndex = 0; // Đếm thứ tự quiz

    void Start()
    {
        if (answerButtons != null && answerButtons.Length > 0)
            defaultBtnColor = answerButtons[0].image.color;
        HideQuiz();
    }

    public void ShowQuizWithCoin(Coin coin)
    {
        if (GameManager.instance != null && GameManager.instance.IsVictoryActive())
            return;

        if (isQuizActive)
            return;

        currentCoin = coin;
        ShowQuiz();
    }

    void ShowQuiz()
    {
        isQuizActive = true;

        if (questions == null || questions.Length == 0)
        {
            Debug.LogWarning("Chưa có câu hỏi quiz nào!");
            return;
        }

        // Lấy lần lượt từng câu hỏi
        currentQuestion = questions[currentQuizIndex];
        correctAnswerIndex = currentQuestion.correctIndex;

        // Tăng chỉ số, nếu hết thì quay lại đầu
        currentQuizIndex++;
        if (currentQuizIndex >= questions.Length)
            currentQuizIndex = 0;

        questionText.text = currentQuestion.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.answers[i];
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            answerButtons[i].image.color = defaultBtnColor;
            answerButtons[i].interactable = true;
        }

        if (resultText != null)
            resultText.text = "";

        quizPanel.alpha = 1;
        quizPanel.interactable = true;
        quizPanel.blocksRaycasts = true;

        if (Player.instance != null)
            Player.instance.enabled = false;
    }

    void OnAnswerSelected(int index)
    {
        foreach (var btn in answerButtons)
            btn.interactable = false;

        if (index == correctAnswerIndex)
        {
            StartCoroutine(BlinkButton(answerButtons[index], Color.green));
            if (resultText != null)
                resultText.text = "<color=green>Đúng rồi!</color>";
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
                GameManager.instance.CheckVictory();
            }
        }
        else
        {
            StartCoroutine(BlinkButton(answerButtons[index], Color.red));
            if (resultText != null)
                resultText.text = "<color=red>Sai rồi!</color>";
            if (GameManager.instance != null)
            {
                GameManager.instance.LoseLife(1);
            }
        }

        if (currentCoin != null)
        {
            Destroy(currentCoin.gameObject);
            currentCoin = null;
        }

        StartCoroutine(HideQuizDelay());
    }

    IEnumerator BlinkButton(Button btn, Color blinkColor)
    {
        float blinkTime = 0.1f;
        int blinkCount = 4;
        Color originalColor = defaultBtnColor;

        for (int i = 0; i < blinkCount; i++)
        {
            btn.image.color = blinkColor;
            yield return new WaitForSecondsRealtime(blinkTime);
            btn.image.color = originalColor;
            yield return new WaitForSecondsRealtime(blinkTime);
        }
        btn.image.color = blinkColor;
    }

    IEnumerator HideQuizDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        HideQuiz();
    }

    void HideQuiz()
    {
        isQuizActive = false;

        quizPanel.alpha = 0;
        quizPanel.interactable = false;
        quizPanel.blocksRaycasts = false;

        if (Player.instance != null)
            Player.instance.enabled = true;
    }
}
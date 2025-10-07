using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text resultText;

    private int correctAnswerIndex;
    private Coin currentCoin;
    private Color defaultBtnColor;

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

        currentCoin = coin;
        ShowQuiz();
    }

    void ShowQuiz()
    {
        questionText.text = "Câu nào là màu của bầu trời?";
        string[] answers = { "Xanh", "Đỏ", "Vàng", "Đen" };
        correctAnswerIndex = 0;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answers[i];
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
        quizPanel.alpha = 0;
        quizPanel.interactable = false;
        quizPanel.blocksRaycasts = false;

        if (Player.instance != null)
            Player.instance.enabled = true;
    }
}
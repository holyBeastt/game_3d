using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Coin xoay liên tục
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 68% xác suất hiện quiz
            if (Random.value < 0.68f)
            {
                QuizManager quiz = FindObjectOfType<QuizManager>();
                if (quiz != null)
                {
                    quiz.ShowQuizWithCoin(this); // Truyền coin vào quiz
                    return; // Không hủy coin, quiz sẽ xử lý
                }
            }
            // Nếu không hiện quiz thì cộng điểm luôn
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
                GameManager.instance.CheckVictory();
            }
            Destroy(gameObject);
        }

        // Nếu coin chạm ground thì tự xóa sau 2 giây
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject, 2);
        }
    }
}
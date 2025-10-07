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
            // Khi player ăn coin → cộng điểm
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
            }

            // Gọi QuizManager (nếu có)
            QuizManager quiz = FindObjectOfType<QuizManager>();
            if (quiz != null)
            {
                quiz.TryShowQuiz(); // 60% xác suất hiển thị quiz
            }

            // Hủy coin sau khi ăn
            Destroy(gameObject);
        }

        // Nếu coin chạm ground thì tự xóa sau 2 giây
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject, 2);
        }
    }
}

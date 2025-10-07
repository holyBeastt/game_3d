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
            // Gọi QuizManager (nếu có)
            QuizManager quiz = FindObjectOfType<QuizManager>();
            if (quiz != null)
            {
                quiz.ShowQuizWithCoin(this); // Truyền coin vào quiz
            }
            else
            {
                // Nếu không có quiz thì hủy coin luôn
                Destroy(gameObject);
            }
        }

        // Nếu coin chạm ground thì tự xóa sau 2 giây
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject, 2);
        }
    }
}
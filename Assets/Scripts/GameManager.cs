using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;

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
        scoreText.SetText(score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText(score.ToString());
    }
}

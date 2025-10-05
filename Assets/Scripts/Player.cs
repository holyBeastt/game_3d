using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    private Rigidbody rb;
    [SerializeField] float speed = 20f;
    //jump
    [SerializeField] float jumpForce = 12f;
    private bool isGrounded = false;
    private Animator animator;
    private bool isALive = true;

    // Lane system
    [SerializeField] float laneDistance = 1.3f; // Khoảng cách giữa các lane
    [SerializeField] float laneOffset = -0.28f; // Dịch tất cả các lane sang trái/phải
    private int currentLane = 1; // 0 = trái, 1 = giữa, 2 = phải
    private float targetX; // Vị trí X mục tiêu
    [SerializeField] float laneChangeSpeed = 10f; // Tốc độ chuyển lane

    // Public getters để GroundTile có thể truy cập
    public float LaneDistance => laneDistance;
    public float LaneOffset => laneOffset;

    // Touch/Swipe controls
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    [SerializeField] float swipeThreshold = 50f;
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();

        // Khởi tạo vị trí ban đầu ở lane giữa
        targetX = (currentLane - 1) * laneDistance + laneOffset;
        currentLane = 1;
    }

    void Update()
    {
        // Nếu game đã kết thúc (victory), không cho phép input
        if (GameManager.instance != null && GameManager.instance.gameEnded)
        {
            return;
        }
        
        // Desktop controls (keyboard/gamepad)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1); // Chuyển sang lane trái
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1); // Chuyển sang lane phải
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Mobile controls (touch/swipe)
        HandleTouchInput();

        if (transform.position.y < -5)
        {
            Die();
        }
    }

    void HandleTouchInput()
    {
        // Handle touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    ProcessSwipe();
                    break;
            }
        }
    }

    void ProcessSwipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;
        float swipeDistance = swipeDirection.magnitude;

        // Check if swipe is long enough
        if (swipeDistance < swipeThreshold) return;

        // Normalize direction
        swipeDirection.Normalize();

        // Determine swipe direction
        if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
        {
            // Vertical swipe
            if (swipeDirection.y > 0 && isGrounded)
            {
                // Swipe up = Jump
                Jump();
            }
        }
        else
        {
            // Horizontal swipe - chuyển lane
            if (swipeDirection.x > 0)
            {
                // Swipe right = chuyển sang lane phải
                ChangeLane(1);
            }
            else if (swipeDirection.x < 0)
            {
                // Swipe left = chuyển sang lane trái
                ChangeLane(-1);
            }
        }
    }

    void ChangeLane(int direction)
    {
        // direction: -1 = trái, 1 = phải
        int newLane = currentLane + direction;

        // Kiểm tra giới hạn lane (0 = trái, 1 = giữa, 2 = phải)
        if (newLane >= 0 && newLane <= 2)
        {
            currentLane = newLane;
            targetX = (currentLane - 1) * laneDistance + laneOffset; // -1 để lane giữa ở vị trí 0, + offset để dịch
        }
    }


    private void FixedUpdate()
    {
        if (!isALive) return;
        
        // Nếu game đã kết thúc (victory), dừng di chuyển
        if (GameManager.instance != null && GameManager.instance.gameEnded)
        {
            return;
        }

        // Di chuyển tiến về phía trước
        Vector3 forward = transform.forward * speed * Time.fixedDeltaTime;

        // Smooth movement giữa các lane
        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        Vector3 horizontalMove = transform.right * (newX - currentX);

        rb.MovePosition(rb.position + forward + horizontalMove);
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public void Die()
    {
        if (!isALive) return; // Tránh gọi nhiều lần
        
        // Nếu game đã kết thúc (victory), không cho phép chết
        if (GameManager.instance != null && GameManager.instance.gameEnded)
        {
            return;
        }

        isALive = false;

        ShowGameOver();
    }

    private void ShowGameOver()
    {
        if (GameUI.instance != null)
        {
            GameUI.instance.ShowGameOver();
        }
        else
        {
            // Fallback nếu không có GameUI
            SceneManager.LoadScene("SampleScene");
        }
    }
}
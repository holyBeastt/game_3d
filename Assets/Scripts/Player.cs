using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    private Rigidbody rb;
    private Animator animator;
    private bool isALive = true;
    private bool isGrounded = false;

    // Speed / acceleration
    public enum SpeedMode { LinearTime, DistanceBased, Curve }
    [Header("Speed Settings")]
    [SerializeField] SpeedMode speedMode = SpeedMode.LinearTime;

    [SerializeField] float baseSpeed = 5f;          // tốc độ ban đầu (hiển thị/ chỉnh Inspector)
    [SerializeField] float speed = 5f;              // tốc độ hiện tại (sẽ cập nhật runtime)
    [SerializeField] float maxSpeed = 60f;           // giới hạn trên
    [SerializeField] float acceleration = 0.3f;        // units per second^2 (dùng cho LinearTime)
    [SerializeField] float speedPerMeter = 0.05f;    // (dùng cho DistanceBased: tăng từng mét)
    [SerializeField] AnimationCurve speedCurve;      // (dùng cho Curve)
    [SerializeField] float curveTimeScale = 60f;     // thời gian tương ứng full length của curve
    [SerializeField] float maxExtraSpeed = 40f;      // tối đa extra khi dùng curve

    private float elapsedTime = 0f;
    private float distanceTravelled = 0f;

    // jump
    [SerializeField] float jumpForce = 12f;

    // Lane system
    [SerializeField] float laneDistance = 1.3f;
    [SerializeField] float laneOffset = -0.28f;
    private int currentLane = 1; // 0 = trái, 1 = giữa, 2 = phải
    private float targetX;
    [SerializeField] float laneChangeSpeed = 10f;

    // Touch
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    [SerializeField] float swipeThreshold = 50f;

    // Public getters
    public float LaneDistance => laneDistance;
    public float LaneOffset => laneOffset;

    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();

        // đảm bảo currentLane được khởi trước khi tính targetX
        currentLane = 1;
        targetX = (currentLane - 1) * laneDistance + laneOffset;

        // khởi tạo tốc độ
        speed = baseSpeed;
        elapsedTime = 0f;
        distanceTravelled = 0f;
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.gameEnded) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();

        HandleTouchInput();

        if (transform.position.y < -5) Die();
    }

    void HandleTouchInput()
    {
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
        if (swipeDistance < swipeThreshold) return;
        swipeDirection.Normalize();

        if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x))
        {
            if (swipeDirection.y > 0 && isGrounded) Jump();
        }
        else
        {
            if (swipeDirection.x > 0) ChangeLane(1);
            else if (swipeDirection.x < 0) ChangeLane(-1);
        }
    }

    void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        if (newLane >= 0 && newLane <= 2)
        {
            currentLane = newLane;
            targetX = (currentLane - 1) * laneDistance + laneOffset;
        }
    }

    private void FixedUpdate()
    {
        if (!isALive) return;
        if (GameManager.instance != null && GameManager.instance.gameEnded) return;

        // CẬP NHẬT TỐC ĐỘ TRƯỚC KHI DI CHUYỂN
        UpdateSpeed();

        // Di chuyển tiến về phía trước
        Vector3 forward = transform.forward * speed * Time.fixedDeltaTime;

        // Smooth chuyển lane (X)
        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        Vector3 horizontalMove = transform.right * (newX - currentX);

        rb.MovePosition(rb.position + forward + horizontalMove);

        // cập nhật quãng đường (nếu dùng distance-based)
        // (đã cập nhật trong UpdateSpeed khi cần; tùy cách bạn muốn tổ chức)
    }

    void UpdateSpeed()
    {
        switch (speedMode)
        {
            case SpeedMode.LinearTime:
                elapsedTime += Time.fixedDeltaTime;
                speed = Mathf.Clamp(speed + acceleration * Time.fixedDeltaTime, baseSpeed, maxSpeed);
                break;

            case SpeedMode.DistanceBased:
                // cộng quãng đường theo tốc độ hiện tại
                distanceTravelled += speed * Time.fixedDeltaTime;
                speed = Mathf.Clamp(baseSpeed + distanceTravelled * speedPerMeter, baseSpeed, maxSpeed);
                break;

            case SpeedMode.Curve:
                elapsedTime += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsedTime / curveTimeScale); // 0..1
                float curveVal = speedCurve != null ? speedCurve.Evaluate(t) : 0f;
                speed = Mathf.Clamp(baseSpeed + curveVal * maxExtraSpeed, baseSpeed, maxSpeed);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    void Jump()
    {
        if (animator != null) animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Die()
    {
        if (!isALive) return;
        if (GameManager.instance != null && GameManager.instance.gameEnded) return;
        isALive = false;
        ShowGameOver();
    }

    private void ShowGameOver()
    {
        if (GameUI.instance != null) GameUI.instance.ShowGameOver();
        else SceneManager.LoadScene("SampleScene");
    }
}

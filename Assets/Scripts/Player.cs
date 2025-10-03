using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float speed = 20f;
    //jump
    [SerializeField] float jumpForce = 12f;
    private bool isGrounded = false;
    private float horizontalInput;
    private Animator animator;
    private bool isALive = true;
    
    // Touch/Swipe controls
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    [SerializeField] float swipeThreshold = 50f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Desktop controls (keyboard/gamepad)
        horizontalInput = Input.GetAxis("Horizontal");
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
            // Horizontal swipe
            if (swipeDirection.x > 0)
            {
                // Swipe right = Move right
                horizontalInput = 1f;
                // Reset after a short time
                Invoke(nameof(ResetHorizontalInput), 0.2f);
            }
            else if (swipeDirection.x < 0)
            {
                // Swipe left = Move left
                horizontalInput = -1f;
                // Reset after a short time
                Invoke(nameof(ResetHorizontalInput), 0.2f);
            }
        }
    }
    
    void ResetHorizontalInput()
    {
        // Only reset if not using keyboard input
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            horizontalInput = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isALive) return;
        Vector3 forward = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime;
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
        isALive = false;
        GameOver();

    }
    public void GameOver()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
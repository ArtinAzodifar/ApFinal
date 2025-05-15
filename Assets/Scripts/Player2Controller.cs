using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Vector2 movingInput;

    private bool isGrounded = false;
    [SerializeField] private Transform groundCheckCollider;
    private const float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;

    // Jump tracking
    private bool isJumping = false;
    private float jumpStartTime;
    private float jumpDuration;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");

            // Mark jump start
            isJumping = true;
            jumpStartTime = Time.time;
        }
    }

    void Update()
    {
        GroundCheck();
        Move();
        UpdateJumpAnimation();
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);

        // Landing detection
        if (isGrounded && !wasGrounded && isJumping)
        {
            isJumping = false;
            jumpDuration = Time.time - jumpStartTime;

            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsJumpingDown", false);
            animator.SetTrigger("Land");

            // Reset animation speed
            animator.speed = 1f;
        }
    }

    private void Move()
    {
        // Flip sprite
        if (movingInput.x > 0)
            transform.localScale = new Vector3(2, 2, 2);
        else if (movingInput.x < 0)
            transform.localScale = new Vector3(-2, 2, 2);

        // Running animation
        bool isRunning = Mathf.Abs(movingInput.x) > 0.01f;
        animator.SetBool("IsRun", isRunning);

        // Move player
        Vector2 move = new Vector2(movingInput.x, 0) * (speed * Time.deltaTime);
        transform.Translate(move);
    }

    private void UpdateJumpAnimation()
    {
        if (!isJumping) return;

        float elapsed = Time.time - jumpStartTime;
        float verticalVelocity = rb.linearVelocity.y;

        if (verticalVelocity > 0.01f)
        {
            // Player moving up
            animator.SetBool("IsJumpingUp", true);
            animator.SetBool("IsJumpingDown", false);

            // Adjust jump up animation speed to fit half the airtime
            float expectedUpTime = jumpDuration * 0.5f;
            animator.speed = expectedUpTime > 0 ? (elapsed / expectedUpTime) : 1f;
        }
        else if (verticalVelocity < -0.01f)
        {
            // Player moving down
            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsJumpingDown", true);

            // Adjust jump down animation speed to fit other half of airtime
            float expectedDownTime = jumpDuration * 0.5f;
            animator.speed = expectedDownTime > 0 ? ((elapsed - expectedDownTime) / expectedDownTime) : 1f;
        }
        else
        {
            // In case velocity near zero mid-air, reset to normal speed
            animator.speed = 1f;
        }
    }

    private void takeDamage()
    {
        animator.SetTrigger("Damage");
    }
}

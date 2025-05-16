using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    public Vector2 movingInput;

    private bool isGrounded;
    [SerializeField] private Transform groundCheckCollider;
    private const float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private int jumpCount;
    private const int maxJumps = 2;

    Animator animator;
    private Rigidbody2D rb;
    
    [SerializeField] private GameObject arrow;
    private bool canShoot = true;

    void Awake()
    {
        isGrounded = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;

            animator.SetTrigger("Jump");
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot)
        {
            canShoot = false; // block further shots
            animator.SetTrigger("Shot");
            StartCoroutine(DelayedArrowShot(0.6f)); // delay to shoot
            StartCoroutine(ResetShotCooldown(1.2f)); // total animation duration (adjust as needed)
        }
    }


    private IEnumerator DelayedArrowShot(float delay)
    {
        yield return new WaitForSeconds(delay);

        float direction = Mathf.Sign(transform.localScale.x); // +1 for right, -1 for left

        Vector3 offset = new Vector3(1.5f * direction, 0.15f, 0f); // relative to player
        Vector3 spawnPos = transform.position + offset;

        GameObject newArrow = Instantiate(arrow, spawnPos, Quaternion.identity);

        // Flip arrow to match direction
        Vector3 arrowScale = newArrow.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        newArrow.transform.localScale = arrowScale;
    }

    private IEnumerator ResetShotCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canShoot = true;
    }
    
    public void groundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            groundCheckCollider.position, groundCheckRadius, groundLayer
        );

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player1") || ((1 << col.gameObject.layer) & groundLayer) != 0)
            {
                isGrounded = true;
                break;
            }
        }
        
        if (isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            jumpCount = 0;
        }
    }


    void FixedUpdate()
    {
        groundCheck();
        move();
    }

    private void move()
    {
        if (movingInput.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (movingInput.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }

        bool isRunning = Mathf.Abs(movingInput.x) > 0.01f;
        animator.SetBool("IsRun", isRunning);

        Vector2 move = new Vector2(movingInput.x, 0) * (speed * Time.deltaTime);
        transform.Translate(move);
    }
}

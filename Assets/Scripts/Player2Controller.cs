using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Vector2 movingInput;
    
    private bool isGrounded = false;
    [SerializeField] private Transform groundCheckCollider;
    [SerializeField] private const float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    Animator animator;
    
    void Awake()
    {
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void groundCheck()
    {
        isGrounded = false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) isGrounded = true;
    }

    void Update()
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
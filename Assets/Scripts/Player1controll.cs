using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 750f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 movingInput;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private Animator animator;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isGrounded;

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            animator.SetTrigger("Jump");//should be changed
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && movingInput.x != 0)
        {
            StartCoroutine(Dash());
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isDashing)
        {
            return;
        }
        GroundCheck();
        move();
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float tempG = rb.gravityScale;
        rb.gravityScale = 0;
        tr.emitting = true;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
        yield return new WaitForSeconds(0.25f);

        tr.emitting = false;
        rb.gravityScale = tempG;
        isDashing = false;
        rb.linearVelocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.5f);

        canDash = true;
    }

    private void move()
    {
        animator.SetBool("Run", movingInput.x != 0);
        //character direction
        transform.localScale = movingInput.x > 0 ? new Vector3(1, 1, 1) : movingInput.x < 0 ? transform.localScale = new Vector3(-1, 1, 1) : transform.localScale = transform.localScale;
        rb.linearVelocity = new Vector2(movingInput.x * speed, rb.linearVelocity.y);
    }
}

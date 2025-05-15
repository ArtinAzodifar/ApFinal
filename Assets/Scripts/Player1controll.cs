using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float dashForce = 15f;
    private Vector2 movingInput;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private Animator animator;
    private bool inDash = false;
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
            animator.SetTrigger("Jump");
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
        if (inDash)
        {
            return;
        }
        move();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private IEnumerator Dash()
    {
        inDash = true;
        canDash = false;
        float tempG = rb.gravityScale;
        rb.gravityScale = 0;
        tr.emitting = true;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
        yield return new WaitForSeconds(0.25f);
        tr.emitting = false;
        rb.gravityScale = tempG;
        inDash = false;
        rb.linearVelocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.5f);
        canDash = true;

    }

    private void move()
    {
        animator.SetBool("Run", movingInput.x != 0);
        //character direction
        if (movingInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movingInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        rb.linearVelocity = new Vector2(movingInput.x * speed, rb.linearVelocity.y);

    }
}

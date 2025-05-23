using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;


public class BaseControll : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 750f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGrounded;
    private Vector2 movingInput;
    [SerializeField] private float knockbackForce;
    private float knockbackTime = 0;
    private float knockbackTotalTime = 0.2f;
    private bool knockFromRight;
    private const float SCALE = 2.2f;

    //inputs:
    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }
    public virtual void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            animator.SetTrigger("Jump");//should be changed
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
    }

    //unity events:
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = gameObject.CompareTag("Player1") ? 3.5f : 5f;
    }
    public virtual void Update()
    {
        Move();
    }
    public void FixedUpdate()
    {
        GroundCheck();
    }

    //methods:
    public void Move()
    {
        animator.SetBool("Run", movingInput.x != 0);
        //character direction
        transform.localScale = movingInput.x > 0 ? new Vector3(SCALE, SCALE, SCALE) : movingInput.x < 0 ? transform.localScale = new Vector3(-SCALE, SCALE, SCALE) : transform.localScale = transform.localScale;
        if (knockbackTime <= 0)
        {
            rb.linearVelocity = new Vector2(movingInput.x * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = knockFromRight ? new Vector2(-knockbackForce, knockbackForce) : new Vector2(knockbackForce, knockbackForce);
            knockbackTime -= Time.deltaTime;
        }
    }
    public void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = colliders.Length > 0;
    }

    //getters:
    public bool IsRunning()
    {
        return movingInput.x != 0;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    //setters:
    public void setKnockbackTime()
    {
        knockbackTime = knockbackTotalTime;
    }
    public void setKnockFromRight(bool b)
    {
        knockFromRight = b;
    }
    public void setKnockbackForce(float f)
    {
        knockbackForce = f;
    }

}

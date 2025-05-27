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
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isGrounded;
    private Vector2 movingInput;
    private bool knockFromRight;
    private bool inKnock = false;
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
            animator.SetTrigger("Jump");
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
    }

    //unity events:
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = gameObject.CompareTag("Player1") ? 6f : 7.5f;
        jumpForce = gameObject.CompareTag("Player1") ? 1200f : 880f;
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
        if (!inKnock)
        {
            rb.linearVelocity = new Vector2(movingInput.x * speed, rb.linearVelocity.y);   
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
    //setters:
    public void setKnockFromRight(bool value)
    {
        knockFromRight = value;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    public IEnumerator KnockBack(float knockbackForce)
    {
        inKnock = true;
        rb.linearVelocity = Vector2.zero;
        
        Vector2 direction = knockFromRight ? new Vector2(-1, 1f) : new Vector2(1, 1f);

        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.2f);

        inKnock = false;
    }

}

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

    //inputs:
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
        else if (!isGrounded)
        {
            Debug.Log("not on ground");
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
        transform.localScale = movingInput.x > 0 ? new Vector3(1, 1, 1) : movingInput.x < 0 ? transform.localScale = new Vector3(-1, 1, 1) : transform.localScale = transform.localScale;
        rb.linearVelocity = new Vector2(movingInput.x * speed, rb.linearVelocity.y);
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

}

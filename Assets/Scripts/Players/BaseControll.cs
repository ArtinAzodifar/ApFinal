using System;
using System.Collections;
using NUnit.Framework;
using Unity.Cinemachine;
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
    protected bool inKnock = false;
    private bool isInDamage = false;
    private const float SCALE = 2.2f;
    
    protected CinemachineCamera vcam;

    //inputs:
    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }
    public virtual void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isInDamage && !inKnock)
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
        jumpForce = gameObject.CompareTag("Player1") ? 900f : 680f;
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
        if (isInDamage || inKnock) return;
        animator.SetBool("Run", movingInput.x != 0);
        //character direction
        transform.localScale = movingInput.x > 0 ? new Vector3(SCALE, SCALE, SCALE) : movingInput.x < 0 ? transform.localScale = new Vector3(-SCALE, SCALE, SCALE) : transform.localScale = transform.localScale;
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
    public float GetJumpForce()
    {
        return jumpForce;
    }

    public bool IsInDamage()
    {
        return isInDamage;
    }
    //setters:
    public void setKnockFromRight(bool value)
    {
        knockFromRight = value;
    }

    public void setIsInDamage(bool value)
    {
        isInDamage = value;
    }

    public void startKnock(float knockbackForce)
    {
        StartCoroutine(KnockBack(knockbackForce));
    }

    private IEnumerator KnockBack(float knockbackForce)
    {
        inKnock = true;
        rb.linearVelocity = Vector2.zero;
        
        Vector2 direction = knockFromRight ? new Vector2(-1, 1f) : new Vector2(1, 1f);

        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(Vector2.zero, ForceMode2D.Impulse);
        inKnock = false;
    }

    public void Teleport(Vector2 position)
    {
        transform.position = position;
    }
}

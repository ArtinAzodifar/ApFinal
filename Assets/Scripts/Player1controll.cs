using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 2000f;
    private Vector2 movingInput;
    private Boolean inDash = false;
    private Boolean isGrounded;

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
    
    public void OnDash(InputAction.CallbackContext context) {
        if (context.performed)
        {
            inDash = true;
        }
        else if (context.canceled)
        {
            inDash = false;
        }
    }

    public void Update()
    {
        move();
    }

    private void move()
    {
        float moveSpeed = inDash ? speed * 2 : speed;
        //character direction
        if (movingInput.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (movingInput.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        Vector2 move = new Vector2(movingInput.x, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
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
}

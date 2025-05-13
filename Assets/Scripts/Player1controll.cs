using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    private Vector2 movingInput;
    private Boolean inDash = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
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
        float MoveSpeed = inDash ? speed * 2 : speed;
        //character direction
        if (movingInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movingInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        Vector2 move = new Vector2(movingInput.x, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(move);
    }
}

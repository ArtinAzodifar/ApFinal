using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : MonoBehaviour
{
    [SerializeField] private float speed = 3.5f;
    private Vector2 movingInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        movingInput = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        move();
    }

    private void move()
    {
        //character direction
        if (movingInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movingInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        Vector2 move = new Vector2(movingInput.x, 0) * speed * Time.deltaTime;
        transform.Translate(move);
    }
}

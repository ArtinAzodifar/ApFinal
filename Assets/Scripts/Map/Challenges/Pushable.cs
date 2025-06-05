using UnityEngine;

public class Pushable : MonoBehaviour
{
    private bool _canMove;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            _canMove = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            _canMove = false;
            _rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (_canMove)
        {
            _rb.AddForce(Vector2.right);
        }
    }
}
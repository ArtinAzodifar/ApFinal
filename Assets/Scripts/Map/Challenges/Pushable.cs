using UnityEngine;

public class Pushable : MonoBehaviour
{
    private bool _canMove = false;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            if (Mathf.Abs(collision.gameObject.transform.position.y - transform.position.y) < 1)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                _canMove = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            _canMove = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            Debug.Log("move");
            _rb.AddForce(Vector2.right * 5f, ForceMode2D.Force);
        }
    }
}
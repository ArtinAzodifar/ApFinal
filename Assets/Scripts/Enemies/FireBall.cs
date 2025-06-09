using System;
using Unity.Cinemachine;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector3 startPos;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        rb.linearVelocity = Vector2.left * transform.localScale.x * speed;
        float xDistance = startPos.x - transform.position.x;
        if (Mathf.Abs(xDistance) >= 30)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            if (collision.GetComponent<Damagable>() != null)
            {
                collision.GetComponent<Damagable>().Damage(damageAmount);
            }
        }

        if (collision.gameObject.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
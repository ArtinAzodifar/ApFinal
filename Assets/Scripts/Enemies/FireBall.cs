using System;
using Unity.Cinemachine;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        rb.linearVelocity = Vector2.left * transform.localScale.x * speed;
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

        if (!collision.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
}

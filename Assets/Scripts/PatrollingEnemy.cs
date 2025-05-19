using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PatrollingEnemy : MonoBehaviour, Damagable
{
    private int health = 3;
    private float speed = 3.5f;
    private Rigidbody2D rb;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;
    private bool isMovingRight = true;
    private bool isChasing = false;
    
    //unity events:
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float player1Distance = transform.position.x - player1.position.x;
        float player2Distance = transform.position.x - player2.position.x;
        if (Mathf.Abs(player1Distance) <= 2 ||
            Mathf.Abs(player2Distance) <= 2)
        {
            isChasing = true;
            if (Mathf.Abs(player1Distance) < Mathf.Abs(player2Distance))//nazdik tare = p1
            {
                if (player1Distance < 0)
                {
                    isMovingRight = true;
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    isMovingRight = false;
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }
            else
            {
                if (player2Distance < 0)
                {
                    isMovingRight = true;
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    isMovingRight = false;
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        Chase();
        Move();
    }
    
    //methods:
    private void Move()
    {
        if (isChasing)
        {
            return;
        }
        if (isMovingRight && transform.position.x >= pointB.position.x)
        {
            isMovingRight = false;
            Flip();
        }
        else if (!isMovingRight && transform.position.x <= pointA.position.x)
        {
            isMovingRight = true;
            Flip();
        }
        
        rb.linearVelocity = new Vector2((isMovingRight ? 1 : -1) * speed, 0);
    }

    private void Chase()
    {
        if (!isChasing)
        {
            return;
        }
        rb.linearVelocity = new Vector2((isMovingRight ? 1 : -1) * speed, 0);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

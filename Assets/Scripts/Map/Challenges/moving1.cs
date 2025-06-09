using System;
using UnityEngine;

public class moving1 : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
     private bool movingToStart = false;
     private bool shouldMoveToEnd = false;
     private bool player1Touched = false;
     private bool player2Touched = false;

    void Update()
    {
        if (movingToStart)
        {
            MoveTo(startPoint.position);

            if (Vector2.Distance(transform.position, startPoint.position) < 0.01f)
            {
                movingToStart = false;
            }
        }
        else if (shouldMoveToEnd)
        {
            MoveTo(endPoint.position);
        }
    }

    private void MoveTo(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            player1Touched = true;
            collision.transform.SetParent(transform);
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            player2Touched = true;
            collision.transform.SetParent(transform);
        }

        if (player1Touched && player2Touched)
        {
            shouldMoveToEnd = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            collision.transform.SetParent(null);
        }
    }

    public void setMovingToStart(bool movingToStart)
    {
        this.movingToStart = movingToStart;
    }
}

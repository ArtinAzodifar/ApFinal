using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class moving1 : NetworkBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private List<GameObject> connectedPlayers = new();

    private bool movingToStart = false;
    private bool shouldMoveToEnd = false;
    private bool player1Touched = false;
    private bool player2Touched = false;

    private Rigidbody2D rb;
    private Vector2 lastPosition;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        lastPosition = (Vector2)transform.position;
    }

    void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (movingToStart)
        {
            if (Vector2.Distance(transform.position, startPoint.position) < 0.01f)
            {
                movingToStart = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        Vector2 current = rb.position;
        Vector2 target = ComputeTargetPosition(current);

        rb.MovePosition(target);

        Vector2 delta = target - lastPosition;

        if (delta != Vector2.zero && connectedPlayers.Count > 0)
        {
            foreach (var go in connectedPlayers)
            {
                if (go == null) continue;

                Rigidbody2D prb = go.GetComponent<Rigidbody2D>();
                if (prb != null) prb.MovePosition(prb.position + delta);
            }
        }

        lastPosition = target;
    }

    private Vector2 ComputeTargetPosition(Vector2 current)
    {
        return movingToStart ? Vector2.MoveTowards(current, startPoint.position, speed * Time.fixedDeltaTime) : (shouldMoveToEnd ? Vector2.MoveTowards(current, endPoint.position, speed * Time.fixedDeltaTime) : current);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.gameObject.CompareTag("Player1"))
        {
            player1Touched = true;
            if (!connectedPlayers.Contains(collision.gameObject)) connectedPlayers.Add(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            player2Touched = true;
            if (!connectedPlayers.Contains(collision.gameObject)) connectedPlayers.Add(collision.gameObject);
        }

        if (player1Touched && player2Touched)
        {
            shouldMoveToEnd = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (connectedPlayers.Contains(collision.gameObject)) connectedPlayers.Remove(collision.gameObject);
        }
    }

    public void setMovingToStart(bool movingToStart)
    {
        this.movingToStart = movingToStart;
    }
}

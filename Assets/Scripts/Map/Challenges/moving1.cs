using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class moving1 : NetworkBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
     private bool movingToStart = false;
     private bool shouldMoveToEnd = false;
     private bool player1Touched = false;
    private bool player2Touched = false;
    private GameManager gameManager;

    private List<GameObject> connectedPlayers = new List<GameObject>();
    private Vector3 lastPos;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }
    public void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

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

    public void LateUpdate()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        Vector3 deltaPos = transform.position - lastPos;

        foreach (var player in connectedPlayers)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            rb.MovePosition(rb.position + (Vector2)deltaPos);
        }

        lastPos = transform.position;
    }

    private void MoveTo(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
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

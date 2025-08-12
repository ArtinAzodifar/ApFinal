using System;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

public class Moving2 : NetworkBehaviour
{
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;
    [SerializeField] private float speed;

    private List<GameObject> connectedPlayers = new();
    
    private Vector3 nexPos;

    private Rigidbody2D rb;
    private Vector2 lastPosition;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        lastPosition = (Vector2)transform.position;
    }

    private void Start()
    {
        nexPos = PointB.position;
    }

    void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;


        if (Vector3.Distance(transform.position, nexPos) <= 0.01f)
        {
            nexPos = (nexPos == PointA.position) ? PointB.position : PointA.position;
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        rb.MovePosition(nexPos);

        Vector2 delta = (Vector2)nexPos - lastPosition;

        if (delta != Vector2.zero && connectedPlayers.Count > 0)
        {
            foreach (var go in connectedPlayers)
            {
                if (go == null) continue;

                Rigidbody2D prb = go.GetComponent<Rigidbody2D>();
                if (prb != null) prb.MovePosition(prb.position + delta);
            }
        }

        lastPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (!connectedPlayers.Contains(collision.gameObject)) connectedPlayers.Add(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (connectedPlayers.Contains(collision.gameObject)) connectedPlayers.Remove(collision.gameObject);
        }
    }
}

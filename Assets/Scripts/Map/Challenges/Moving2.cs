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

    void FixedUpdate()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (rb == null) return;

        Vector2 current = rb.position;
        Vector2 target = Vector2.MoveTowards(current, (Vector2)nexPos, speed * Time.fixedDeltaTime);

        rb.MovePosition(target);

        Vector2 platformVelocity = (target - lastPosition) / Time.fixedDeltaTime;

        if (platformVelocity != Vector2.zero && connectedPlayers.Count > 0)
        {
            for (int i = connectedPlayers.Count - 1; i >= 0; i--)
            {
                var go = connectedPlayers[i];
                if (go == null) { connectedPlayers.RemoveAt(i); continue; }

                Rigidbody2D prb = go.GetComponent<Rigidbody2D>();
                if (prb != null)
                {
                    Vector2 pv = platformVelocity;
                    pv.y = 0f;
                    prb.linearVelocity += pv;
                }
            }
        }

        lastPosition = target;

        if (Vector2.Distance(target, nexPos) <= 0.01f)
        {
            nexPos = (nexPos == PointA.position) ? PointB.position : PointA.position;
        }
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

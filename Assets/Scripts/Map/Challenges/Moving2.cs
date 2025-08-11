using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Moving2 : NetworkBehaviour
{
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;
    [SerializeField] private float speed;
    private List<GameObject> connectedPlayers = new List<GameObject>();
    private GameManager gameManager;

    private Vector3 nexPos;
    private Vector3 lastPos;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void Start()
    {
        nexPos = PointB.position;
        lastPos = transform.position;
    }

    public void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        transform.position = Vector3.MoveTowards(transform.position, nexPos, speed * Time.deltaTime);

        if (transform.position == nexPos)
        {
            nexPos = (nexPos == PointA.position) ? PointB.position : PointA.position;
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

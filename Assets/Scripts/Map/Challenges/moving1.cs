using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Vivox;

public class moving1 : NetworkBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private bool movingToStart = false;
    private NetworkVariable<bool> shouldMoveToEnd = new(false);
    private NetworkVariable<bool> player1Touched = new(false);
    private NetworkVariable<bool> player2Touched = new(false);

    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
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
        else if (shouldMoveToEnd.Value)
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
        var netObj = collision.gameObject.GetComponent<NetworkObject>();
        if (gameManager.IsLocalMode() || (netObj != null && netObj.IsOwner))
        {
            if (collision.gameObject.CompareTag("Player1"))
            {
                if (gameManager.IsLocalMode()) player1Touched.Value = true;
                else p1touchedServerRpc(true);

                collision.transform.SetParent(transform);
            }

            if (collision.gameObject.CompareTag("Player2"))
            {
                if (gameManager.IsLocalMode()) player2Touched.Value = true;
                else p2touchedServerRpc(true);

                collision.transform.SetParent(transform);
            }

            if (player1Touched.Value && player2Touched.Value)
            {
                if (gameManager.IsLocalMode()) shouldMoveToEnd.Value = true;
                else shouldMoveServerRpc(true);
            }
        }
    }
    [ServerRpc]
    private void p1touchedServerRpc(bool value) { player1Touched.Value = value; }
    [ServerRpc]
    private void p2touchedServerRpc(bool value) { player2Touched.Value = value; }
    [ServerRpc]
    private void shouldMoveServerRpc(bool value) { shouldMoveToEnd.Value = true; }

    public void OnCollisionExit2D(Collision2D collision)
    {
        var netObj = collision.gameObject.GetComponent<NetworkObject>();
        if (gameManager.IsLocalMode() || (netObj != null && netObj.IsOwner))
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                collision.transform.SetParent(null);
            }
        }
    }

    public void setMovingToStart(bool movingToStart)
    {
        this.movingToStart = movingToStart;
    }
}

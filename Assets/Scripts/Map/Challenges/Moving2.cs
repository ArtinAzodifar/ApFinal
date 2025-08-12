using System;
using UnityEngine;
using Unity.Netcode;

public class Moving2 : NetworkBehaviour
{
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;
    [SerializeField] private float speed;
    
    private Vector3 nexPos;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        nexPos = PointB.position;
    }

    void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        transform.position = Vector3.MoveTowards(transform.position, nexPos, speed * Time.deltaTime);

        if (transform.position == nexPos)
        {
            nexPos = (nexPos == PointA.position) ? PointB.position : PointA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var netObj = collision.gameObject.GetComponent<NetworkObject>();
        if (gameManager.IsLocalMode() || (netObj != null && netObj.IsOwner))
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                collision.gameObject.transform.parent = transform;
            }   
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var netObj = collision.gameObject.GetComponent<NetworkObject>();
        if (gameManager.IsLocalMode() || (netObj != null && netObj.IsOwner))
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                collision.gameObject.transform.parent = null;
            }
        }
    }
}

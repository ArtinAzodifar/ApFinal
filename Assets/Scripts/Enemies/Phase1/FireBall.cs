using System;
using Unity.Cinemachine;
using UnityEngine;
using Unity.Netcode;

public class FireBall : NetworkBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        startPos = transform.position;
    }

    public void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        rb.linearVelocity = Vector2.left * transform.localScale.x * speed;
        float xDistance = startPos.x - transform.position.x;
        if (Mathf.Abs(xDistance) >= 30)
        {
            disableObject();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            if (collision.GetComponent<Damagable>() != null)
            {
                collision.GetComponent<Damagable>().Damage(damageAmount);
            }
        }

        if (collision.gameObject.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Ground"))
        {
            disableObject();
        }
    }
    
    private void disableObject()
    {
        if (GameManager.Instance.IsLocalMode()) gameObject.SetActive(false);
        else disableObjectClientRpc();
    }
    [ClientRpc]
    private void disableObjectClientRpc(){ gameObject.SetActive(false); }
}
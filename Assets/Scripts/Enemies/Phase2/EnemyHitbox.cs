using UnityEngine;
using Unity.Netcode;

public class EnemyHitbox : NetworkBehaviour
{
    public CloseRangeEnemy enemyParent;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            enemyParent.ApplyDamageAndKnockback(other);
        }
    }
}
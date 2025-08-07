using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public CloseRangeEnemy enemyParent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            enemyParent.ApplyDamageAndKnockback(other);
        }
    }
}
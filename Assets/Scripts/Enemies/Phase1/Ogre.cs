using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Ogre : BaseMovingEnemy
{
    [SerializeField] private float stopDistance;
    [SerializeField] private int damageAmount;
    [SerializeField] private LayerMask playerLayers;
    private Transform attackZone;
    private float attackRange = 1f;
    private bool isAttacking = false;
    private bool isInCoolDown = false;
    private EnemyHealth enemyHealth;

    public override void Awake()
    {
        base.Awake();
        attackZone = transform.Find("OgreAttackZone");
        enemyHealth = GetComponentInChildren<EnemyHealth>();
    }

    public override void Chase()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (enemyHealth.IsDead()) return;

        animator.SetBool("Run", isChasing && !isAttacking);
        if (!isChasing) return;

        float targetXDistance = transform.position.x - target.transform.position.x;
        float targetYDistance = transform.position.y - target.transform.position.y;
        if (Mathf.Abs(targetXDistance) <= stopDistance && Mathf.Abs(targetYDistance) <= 2)
        {
            setDirection(target);
            rb.linearVelocity = Vector2.zero;
            if (!isAttacking)
            {
                StartAttack();
            }
        }
        else if(!isAttacking)
        {
            isAttacking = false;
            setDirection(target);
            rb.linearVelocity = new Vector2((isMovingRight ? 1 : -1) * speed, 0);
        }
    }

    private void StartAttack()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (enemyHealth.IsDead()) return;

        if (isInCoolDown) return;
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    private void ActiveCollider()//is called in the middle of attack animation event
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (enemyHealth.IsDead()) return;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackZone.position, attackRange, playerLayers);
        foreach (Collider2D player in hitPlayers)
        {

            if (player.gameObject.GetComponent<Damagable>() != null)
            {
                player.gameObject.GetComponent<Damagable>().Damage(damageAmount);
            }
            
            if (gameManager.IsLocalMode())
            {
                BaseControll b = player.gameObject.GetComponent<BaseControll>();
                b.setKnockFromRight(player.gameObject.transform.position.x <= transform.position.x);
                b.startKnock(900);
            }
            else if (IsServer)
            {
                BaseControll b = player.gameObject.GetComponent<BaseControll>();
                b.setKnockFromRightClientRpc(player.gameObject.transform.position.x <= transform.position.x);
                b.startKnockClientRpc(900);
            }
        }
    }
    private void FinishAttack()//is called in the end of attack animation event
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (enemyHealth.IsDead()) return;
        
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        bool oldIsChasing = isChasing;
        isChasing = false;
        isInCoolDown = true;
        yield return new WaitForSeconds(0.7f);
        isInCoolDown = false;
        isChasing = oldIsChasing;
    }
    void OnDrawGizmosSelected()
    {
        if (attackZone == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackZone.position, attackRange);
    }
}
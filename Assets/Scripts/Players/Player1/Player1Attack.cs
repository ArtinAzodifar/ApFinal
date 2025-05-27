using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Attack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private Animator animator;
    private Transform attackZone;
    private float attackRange = 0.9f;
    private bool isAttacking = false;

    //inputs:
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            StartAttack();
        }
    }

    //unity events:
    public void Awake()
    {
        animator = GetComponent<Animator>();
        attackZone = transform.Find("AttackZone");
    }

    //methods:
    private void StartAttack()
    {
        Debug.Log("started attack");
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    private void ActiveCollider()//is called in the middle of attack animation event
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.gameObject.GetComponent<Damagable>().Damage(1);
        }
    }
    private void FinishAttack()//is called in the end of attack animation event
    {
        isAttacking = false;
        Debug.Log("finished attack");
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackZone == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackZone.position, attackRange);
    }
}

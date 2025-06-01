using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player1Attack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int BaseDamageAmount;
    private Animator animator;
    private Transform attackZone;
    private float attackRange = 0.9f;
    private bool isAttacking = false;
    private int damageBoostAmount = 0;
    private Coroutine boostCoroutine;

    private void OnEnable()
    {
        DamageBooster.OnDamageBoost += DamageBoost;
    }

    private void OnDisable()
    {
        DamageBooster.OnDamageBoost -= DamageBoost;
    }

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
            enemy.gameObject.GetComponent<Damagable>().Damage(BaseDamageAmount + damageBoostAmount);
        }
    }
    private void FinishAttack()//is called in the end of attack animation event
    {
        isAttacking = false;
        Debug.Log("finished attack");
    }

    private void DamageBoost(GameObject player, int damage, float time)
    {
        if (player != gameObject) return;
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
        }
        boostCoroutine = StartCoroutine(ApplyBoost(damage, time));
        
    }

    private IEnumerator ApplyBoost(int damage, float time)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        damageBoostAmount += damage;
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = Color.white;
        damageBoostAmount -= damage;
    }
    
    //getters:
    public bool IsAttacking()
    {
        return isAttacking;
    }
}

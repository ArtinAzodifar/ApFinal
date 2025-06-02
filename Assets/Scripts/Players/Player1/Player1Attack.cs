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
            StartCoroutine(Attack());
        }
    }

    //unity events:
    public void Awake()
    {
        animator = GetComponent<Animator>();
        attackZone = transform.Find("AttackZone");
    }

    
    //methods:
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.gameObject.GetComponent<Damagable>().Damage(BaseDamageAmount + damageBoostAmount);
        }
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
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

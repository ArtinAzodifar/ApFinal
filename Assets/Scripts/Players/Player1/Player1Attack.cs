using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Unity.Netcode;

public class Player1Attack : NetworkBehaviour
{
    public static event Action P1Mana;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask leverLayer;
    [SerializeField] private int BaseDamageAmount;
    private Animator animator;
    private Transform attackZone;
    private float attackRange = 0.9f;
    private NetworkVariable<bool> isAttacking = new NetworkVariable<bool>(false);
    private int damageBoostAmount = 0;
    private Coroutine boostCoroutine;
    private GameManager gameManager;

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
        if (!gameManager.IsLocalMode() && !IsOwner) return;
        if (context.performed && !isAttacking.Value && !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage())
        {
            if (gameManager.IsLocalMode()) StartCoroutine(Attack());
            else attackServerRpc();
        }
    }
    [ServerRpc]
    private void attackServerRpc(){StartCoroutine(Attack());}

    //unity events:
    public void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        attackZone = transform.Find("AttackZone");
    }

    
    //methods:
    private IEnumerator Attack()
    {
        //this method applies attack process and lever toggles
        //this IEnumerator is only started in local mode or by *server*
        if (!gameManager.IsLocalMode() && !IsServer) yield break;
        isAttacking.Value = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);

        //attack enemies
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            if (enemy.gameObject.GetComponent<Damagable>() != null)
            {
                enemy.gameObject.GetComponent<Damagable>().Damage(BaseDamageAmount + damageBoostAmount);
            }
            if (gameManager.IsLocalMode()) P1Mana?.Invoke();
            else if(IsServer) invokeManaClientRpc();
        }

        //use levers
        Collider2D[] toggleLever = Physics2D.OverlapCircleAll(attackZone.position, attackRange, leverLayer);
        foreach (Collider2D lever in toggleLever)
        {
            if (lever.gameObject.GetComponent<LeverToggle>() != null)
            {
                if (gameManager.IsLocalMode() || IsServer) lever.gameObject.GetComponent<LeverToggle>().Toggle();
            }
        }
        yield return new WaitForSeconds(0.5f);
        isAttacking.Value = false;
    }
    [ClientRpc]
    private void invokeManaClientRpc(){P1Mana?.Invoke();}

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
    
    //setters:
    public void setIsAttacking(bool value)
    {
        isAttacking.Value = value;
    }
    
    //getters:
    public bool IsAttacking()
    {
        return isAttacking.Value;
    }
}

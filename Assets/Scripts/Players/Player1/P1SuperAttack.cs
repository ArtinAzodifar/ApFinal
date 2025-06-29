using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1SuperAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damageAmount;
    public int mana;
    private int maxMana;
    private Animator animator;
    private ManaBar manaBar;

    //unity events:
    public void Awake()
    {
        animator = GetComponent<Animator>();
        manaBar = GameObject.FindWithTag("MeleeHealth").GetComponentInChildren<ManaBar>();
    }
    public void Start()
    {
        mana = 0;
        maxMana = 10;
        manaBar.SetMaxMana(maxMana);
    }

    private void OnEnable()
    {
        Player1Attack.P1Mana += ManaAdd;
        FullMana.ManaFill += MaxMana;
    }

    private void OnDisable()
    {
        Player1Attack.P1Mana -= ManaAdd;
        FullMana.ManaFill -= MaxMana;
    }
    
    
    //inputs:
    public void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !gameObject.GetComponent<Player1Attack>().IsAttacking() &&
            !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana >= maxMana)
        {
            mana = 0;
            manaBar.SetMaxMana(maxMana);
            StartCoroutine(SuperAttack());
        }
    }

    private IEnumerator SuperAttack()
    {
        gameObject.GetComponent<Player1Attack>().setIsAttacking(true);
        animator.SetTrigger("SuperAttack");
        yield return new WaitForSeconds(0.5f);
        float pastTime = 0;
        float maxTime = 1.7f;
        while (pastTime < maxTime)
        {
            pastTime += Time.deltaTime;
            Vector2 boxCenter = transform.position;
            Vector2 boxSize = new Vector2(9.5f, 2f);
            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);
            foreach (Collider2D enemy in hitEnemy)
            {
                if (enemy.gameObject.GetComponent<Damagable>() != null)
                {
                    enemy.gameObject.GetComponent<Damagable>().Damage(damageAmount);
                }
            }

            yield return null;
        }
        yield return new WaitForSeconds(1.7f);
        gameObject.GetComponent<Player1Attack>().setIsAttacking(false);
    }

    private void ManaAdd()
    {
        mana++;
        manaBar.addMana();
    }

    private void MaxMana(GameObject g)
    {
        if(g != gameObject) return;
        mana = maxMana;
        manaBar.FillMana();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = transform.position;
        Vector2 boxSize = new Vector2(9.5f, 2f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}

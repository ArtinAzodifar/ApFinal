using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1SuperAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damageAmount;
    [SerializeField] private ManaBar manaBar;
    
    public int mana;
    private int maxMana;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Start()
    {
        maxMana = 10;
        manaBar.SetMaxMana(maxMana);

        if (SaveManager.Instance == null || !SaveManager.Instance.IsGameLoaded)
        {
            mana = 0;
            manaBar.SetMana(mana);
        }
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
    
    public void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !gameObject.GetComponent<Player1Attack>().IsAttacking() &&
            !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana >= maxMana)
        {
            mana = 0;
            manaBar.SetMana(mana);
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
    
    public int getMana()
    {
        return mana;
    }

    public void setMana(int amount)
    {
        this.mana = amount;
        manaBar.SetMana(this.mana);
    }
}
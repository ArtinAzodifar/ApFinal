using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class P2SuperShoot : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    public int mana;
    private int maxMana;
    private Animator animator;
    private ManaBar manaBar;
    
    [SerializeField] private GameObject laserSegmentPrefab;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private float segmentSpacing = 1f;

    //unity events:
    public void Awake()
    {
        animator = GetComponent<Animator>();
        manaBar = GameObject.FindWithTag("RangeHealth").GetComponentInChildren<ManaBar>();
    }
    public void Start()
    {
        mana = 0;
        maxMana = 10;
        manaBar.SetMaxMana(maxMana);
    }

    private void OnEnable()
    {
        ArrowController.P2Mana += ManaAdd;
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
        if (context.performed && !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana >= maxMana)
        {
            mana = 0;
            manaBar.SetMaxMana(maxMana);
            StartCoroutine(SuperAttack());
        }
    }

    private IEnumerator SuperAttack()
    {
        animator.SetTrigger("SuperShoot");
        yield return new WaitForSeconds(0.3f);
        float pastTime = 0;
        float maxTime = 1.9f;
        while (pastTime < maxTime)
        {
            pastTime += Time.deltaTime;
            Vector2 boxCenter = transform.position;
            boxCenter.x += gameObject.transform.localScale.x > 0 ? 4.5f : -4.5f;
            Vector2 boxSize = new Vector2(9f, 2f);
            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);
            foreach (Collider2D enemy in hitEnemy)
            {
                if (enemy.gameObject.GetComponent<Damagable>() != null)
                {
                    enemy.gameObject.GetComponent<Damagable>().Damage(10000);
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.9f);
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
        boxCenter.x += gameObject.transform.localScale.x > 0 ? 4.5f : -4.5f;
        Vector2 boxSize = new Vector2(9f, 2f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
    
    private void ShowLaserAnimation()
    {
        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        for (int i = 0; i < laserCount; i++)
        {
            Vector3 spawnPos = laserStartPoint.position + direction * segmentSpacing * i;
            GameObject laserSegment = Instantiate(laserSegmentPrefab, spawnPos, Quaternion.identity);

            if (transform.localScale.x < 0)
            {
                Vector3 scale = laserSegment.transform.localScale;
                scale.x *= -1;
                laserSegment.transform.localScale = scale;
            }
        }
    }

}

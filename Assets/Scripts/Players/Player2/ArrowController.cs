using System;
using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public static event Action P2Mana;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private int DamageAmount = 1;
    private Animator animator;
    private bool canMove = true;
    private bool isCollided = false;
    private float elapsedTime;
    private float totalTime = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isCollided = false;
        canMove = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= totalTime)
        {
            gameObject.SetActive(false);
        }
        if (canMove)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            transform.Translate(Vector2.right * (direction * speed * Time.deltaTime));
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isCollided) return;
        isCollided = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetTrigger("Arrow-hit");
            canMove = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ArrowDamageCooldown(3f));
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Arrow-Damage");
            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(DamageAmount);
            }
            P2Mana?.Invoke();
            StartCoroutine(ArrowDamageCooldown(0.5f));
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        canMove = false;
    }

    private IEnumerator ArrowDamageCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        gameObject.SetActive(false);
    }
    
    //getters:
    public int GetDamageAmount()
    {
        return DamageAmount;
    }
    //setters:
    public void SetDamageAmount(int amount)
    {
        DamageAmount = amount;
    }
}
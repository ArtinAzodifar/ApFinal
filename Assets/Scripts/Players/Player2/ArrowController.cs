using System;
using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed; 
    private int DamageAmount = 1;
    private Animator animator;
    private bool canMove = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            transform.Translate(Vector2.right * (direction * speed * Time.deltaTime));
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetTrigger("Arrow-hit");
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Arrow-Damage");
            collision.gameObject.GetComponent<Damagable>().Damage(DamageAmount);
            StartCoroutine(ArrowDamageCooldown(0.7f));
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

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.SetActive(false);
        }
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
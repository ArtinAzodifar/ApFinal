using System;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class ArrowController : NetworkBehaviour
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
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;
        isCollided = false;
        canMove = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= totalTime)
        {
            disableObject();
        }
        if (canMove)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            transform.Translate(Vector2.right * (direction * speed * Time.deltaTime));
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        if (isCollided) return;
        isCollided = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetTrigger("Arrow-hit");
            canMove = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ArrowDamageCooldown(3f));
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Arrow-Damage");
            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(DamageAmount);
            }
            if (GameManager.Instance.IsLocalMode()) P2Mana?.Invoke();
            else if (IsServer) invokeManaClientRpc();
            StartCoroutine(ArrowDamageCooldown(0.5f));
        }
        else
        {
            disableObject();
        }
        
        canMove = false;
    }
    [ClientRpc]
    private void invokeManaClientRpc()  {P2Mana?.Invoke();}

    private IEnumerator ArrowDamageCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        disableObject();
    }

    private void disableObject()
    {
        if (GameManager.Instance.IsLocalMode()) gameObject.SetActive(false);
        else disableObjectClientRpc();
    }
    [ClientRpc]
    private void disableObjectClientRpc(){ gameObject.SetActive(false); }
    
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
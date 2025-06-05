using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : BaseControll
{
    private ObjectPooler ArrowPooler;
    
    private bool _canDoubleJump;
    private bool isBoosted = false;
    private Coroutine boostCoroutine;
    
    public override void Awake()
    {
        base.Awake();
        ArrowPooler = GameObject.FindWithTag("ArrowPool").GetComponent<ObjectPooler>();
    }
    private void OnEnable()
    {
        DamageBooster.OnDamageBoost += DamageBoost;
    }

    private void OnDisable()
    {
        DamageBooster.OnDamageBoost -= DamageBoost;
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !IsInDamage())
        {
            _canDoubleJump = true;
            base.OnJump(context);
        } 
        else if (context.performed && _canDoubleJump && !IsInDamage())
        {
            _canDoubleJump = false;
            animator.SetTrigger("DoubleJump");//should be changed
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(GetJumpForce() * Vector2.up, ForceMode2D.Impulse);
        }
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && !IsInDamage())
        {
            animator.SetTrigger("Shoot");
        }
    }

    private void Shoot()// is called in the middle of attack animation
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        float offsetX = Mathf.Abs(transform.localScale.x) * 0.5f;
        float offsetY = Mathf.Abs(transform.localScale.y) * 0.1f;

        Vector3 spawnPosition = transform.position + new Vector3(offsetX * direction, offsetY, 0f);
        GameObject newArrow = ArrowPooler.GetObject();
        newArrow.transform.position = spawnPosition;
        newArrow.gameObject.GetComponent<ArrowController>().SetDamageAmount(isBoosted ? 2 : 1);
        Vector3 arrowScale = newArrow.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        newArrow.transform.localScale = arrowScale;
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
        isBoosted = true;
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = Color.white;
        isBoosted = false;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Attack : MonoBehaviour
{
    private Animator animator;
    private Transform attackZone;
    private bool isAttacking = false;

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
    public void Start()
    {
        attackZone.gameObject.SetActive(false);
    }

    //methods:
    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    private void ActiveCollider()//is called in the middle of attack animation event
    {
        attackZone.gameObject.SetActive(true);
    }
    private void FinishAttack()//is called in the end of attack animation event
    {
        isAttacking = false;
        attackZone.gameObject.SetActive(false);
    }
}

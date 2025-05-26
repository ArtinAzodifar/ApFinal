using System.Collections;
using UnityEngine;

public class Ogre : BaseMovingEnemy
{
    [SerializeField] private float stopDistance;
    private Transform attackZone;
    private bool isAttacking = false;
    private bool isInCoolDown = false;

    public override void Awake()
    {
        base.Awake();
        attackZone = transform.Find("OgreAttackZone");
        attackZone.gameObject.SetActive(false);
    }

    public override void Chase()
    {
        animator.SetBool("Run", isChasing && !isAttacking);
        if (!isChasing) return;

        float targetDistance = transform.position.x - target.transform.position.x;
        if (Mathf.Abs(targetDistance) <= stopDistance)
        {
            setDirection(target);
            rb.linearVelocity = Vector2.zero;
            if (!isAttacking)
            {
                StartAttack();
            }
        }
        else if(!isAttacking)
        {
            isAttacking = false;
            setDirection(target);
            rb.linearVelocity = new Vector2((isMovingRight ? 1 : -1) * speed, 0);
        }
    }

    private void StartAttack()
    {
        if (isInCoolDown) return;
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
        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        bool oldIsChasing = isChasing;
        isChasing = false;
        isInCoolDown = true;
        yield return new WaitForSeconds(0.7f);
        isInCoolDown = false;
        isChasing = oldIsChasing;
    }
}
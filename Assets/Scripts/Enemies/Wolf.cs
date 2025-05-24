using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wolf : BaseMovingEnemy
{
    private bool inCoolDown = false;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            BaseControll b = collision.gameObject.GetComponent<BaseControll>();
            b.setKnockbackTime();
            b.setKnockbackForce(10f);
            b.setKnockFromRight(collision.gameObject.transform.position.x <= transform.position.x);
            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(1);
            }
            if(!inCoolDown) StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        inCoolDown = true;
        isChasing = false;
        float oldSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1f);
        isChasing = true;
        speed = oldSpeed;
        inCoolDown = false;
    }
}

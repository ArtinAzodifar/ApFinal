using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wolf : BaseMovingEnemy
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (gameManager.IsLocalMode())
            {
                BaseControll b = collision.gameObject.GetComponent<BaseControll>();
                b.setKnockFromRight(collision.gameObject.transform.position.x <= transform.position.x);
                b.startKnock(900);
            }
            else if (IsServer)
            {
                BaseControll b = collision.gameObject.GetComponent<BaseControll>();
                b.setKnockFromRightClientRpc(collision.gameObject.transform.position.x <= transform.position.x);
                b.startKnockClientRpc(900);
            }

            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(20);
            }
            if (!inCoolDown) StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        inCoolDown = true;
        isChasing = false;
        float oldSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(3f);
        isChasing = true;
        speed = oldSpeed;
        inCoolDown = false;
    }
}

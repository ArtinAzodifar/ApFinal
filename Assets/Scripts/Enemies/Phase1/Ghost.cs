using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Ghost : BaseMovingEnemy
{
    private bool hasHit = false;

    public override void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        FindPlayer();
        Chase();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (hasHit) return;
        hasHit = true;
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(50);
            }
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
            animator.SetTrigger("Vanish");
        }
        if (gameManager.IsLocalMode()) Destroy(gameObject, 1.8f);
        else if (IsServer && GetComponent<NetworkObject>() != null) StartCoroutine(despawnAfter(0.6f));
        else if (IsServer) DestroyClientRpc(0.6f);
    }

    public override void FindPlayer()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        base.FindPlayer();
        if (isChasing)
        {
            animator.SetTrigger("Apear");
        }
    }

    public override void Chase()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        animator.SetBool("Run", isChasing);
        base.Chase();
    }

    private IEnumerator despawnAfter(float time)
    {
        yield return new WaitForSeconds(time);

        if ((bool)GetComponent<NetworkObject>().IsSceneObject) DestroyClientRpc(0);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    private void DestroyClientRpc(float time)
    {
        Destroy(gameObject, time);
    }
    
}

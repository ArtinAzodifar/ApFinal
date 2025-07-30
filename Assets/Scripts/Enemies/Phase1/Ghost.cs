using UnityEngine;

public class Ghost : BaseMovingEnemy
{
    private bool hasHit = false;

    public override void Update()
    {
        FindPlayer();
        Chase();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(hasHit)  return;
        hasHit = true;
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (collision.gameObject.GetComponent<Damagable>() != null)
            {
                collision.gameObject.GetComponent<Damagable>().Damage(50);
            }
            BaseControll b = collision.gameObject.GetComponent<BaseControll>();
            b.setKnockFromRight(collision.gameObject.transform.position.x <= transform.position.x);
            b.startKnock(900);
            animator.SetTrigger("Vanish");
        }
        Destroy(gameObject, 0.6f);
    }

    public override void FindPlayer()
    {
        base.FindPlayer();
        if (isChasing)
        {
            animator.SetTrigger("Apear");
        }
    }

    public override void Chase()
    {
        animator.SetBool("Run", isChasing);
        base.Chase();
    }
    
}

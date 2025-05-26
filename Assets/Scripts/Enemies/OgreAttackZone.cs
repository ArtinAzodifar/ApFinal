using UnityEngine;

public class OgreAttackZone : MonoBehaviour
{
    private bool damaged;

    //unity events:
    public void OnEnable()
    {
        damaged = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter");
        if (damaged)
        {
            return;
        }
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("attack");
            BaseControll b = other.gameObject.GetComponent<BaseControll>();
            b.setKnockFromRight(other.gameObject.transform.position.x <= transform.position.x);
            StartCoroutine(b.KnockBack(900));
            //other.GetComponent<Damagable>().Damage(5);
            damaged = true;
        }
    }
    
    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("stay");
        if (damaged)
        {
            return;
        }
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("attack");
            BaseControll b = other.gameObject.GetComponent<BaseControll>();
            b.setKnockFromRight(other.gameObject.transform.position.x <= transform.position.x);
            StartCoroutine(b.KnockBack(900));
            //other.GetComponent<Damagable>().Damage(5);
            damaged = true;
        }
    }
}
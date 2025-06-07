using System.Collections;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    private bool p1Trapped = false;
    private bool p2Trapped = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            p1Trapped = true;
            StartCoroutine(p1Trap(collision.gameObject));
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            p2Trapped = true;
            StartCoroutine(p2Trap(collision.gameObject));
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            Debug.Log("stop");
            p1Trapped = false;
            StopCoroutine(p1Trap(collision.gameObject));
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            p2Trapped = false;
            StopCoroutine(p2Trap(collision.gameObject));
        }
    }

    private IEnumerator p1Trap(GameObject g)
    {
        while (p1Trapped)
        {
            g.GetComponent<Damagable>().Damage(damageAmount);
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator p2Trap(GameObject g)
    {
        while (p2Trapped)
        {
            g.GetComponent<Damagable>().Damage(damageAmount);
            yield return new WaitForSeconds(1f);
        }
    }
    
}

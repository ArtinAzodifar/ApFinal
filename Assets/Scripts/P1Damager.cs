using UnityEngine;

public class P1Damager : MonoBehaviour
{
    private bool damaged;

    //unity events:
    public void OnEnable()
    {
        damaged = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (damaged)
        {
            return;
        }
        if (other.CompareTag("enemy"))
        {
            other.GetComponent<Damagable>().Damage(1);
            damaged = true;
        }
    }
}

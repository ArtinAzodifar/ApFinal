using UnityEngine;

public class EnemyHealth : MonoBehaviour, Damagable
{
    [SerializeField] private int health;

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
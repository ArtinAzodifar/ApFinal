using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Damagable
{
    [SerializeField] private int health;
    [SerializeField] private EnemyHB healthBar;


    public void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    public void Damage(int amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
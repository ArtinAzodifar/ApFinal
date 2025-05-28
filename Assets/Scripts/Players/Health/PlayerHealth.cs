using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Damagable
{
    private Animator animator;
    [SerializeField] private PlayerHB healthBar;
    [SerializeField] private HealthPoint healthPoint;
    private int lives = 3;
    private int Health = 100;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        healthBar.SetMaxHealth(Health);
        healthBar.SetHealth(Health);
        healthPoint.SetLives(lives);
    }

    public void Damage(int amount)
    {
        Health -= amount;
        healthBar.SetHealth(Health);
        if (Health <= 0)
        {
            Health = 100;
            healthBar.SetHealth(Health);
            lives--;
            healthPoint.ExplodeHeart(lives);
        }

        if (lives <= 0)
        {
            //animator.SetTrigger("Death");
            Debug.Log("game over");
            //contrtoller: lose
        }
    }
}

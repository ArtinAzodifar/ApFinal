using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Damagable
{
    private Animator animator;
    [SerializeField] private PlayerHB healthBar;
    private int lives = 3;
    private int Health = 100;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        Debug.Log(lives);
        healthBar.SetMaxHealth(Health);
        healthBar.SetHealth(Health);
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
            Debug.Log(lives);
        }

        if (lives <= 0)
        {
            //animator.SetTrigger("Death");
            Debug.Log("game over");
            //contrtoller: lose
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, Damagable
{
    private Animator animator;
    [SerializeField] private String healthTag;
    private PlayerHB healthBar;
    private HealthPoint healthPoint;
    private int lives = 3;
    private int Health = 100;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        healthBar = GameObject.FindWithTag(healthTag).GetComponentInChildren<PlayerHB>();
        healthPoint = GameObject.FindWithTag(healthTag).GetComponentInChildren<HealthPoint>();
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

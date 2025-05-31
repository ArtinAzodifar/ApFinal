using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, Damagable
{
    private Animator animator;
    [SerializeField] private String healthTag;
    private PlayerHB healthBar;
    private HealthPoint healthPoint;
    [SerializeField] private int maxLives;
    [SerializeField] private int MaxHealth;
    private int Health;
    private int lives = 3;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        healthBar = GameObject.FindWithTag(healthTag).GetComponentInChildren<PlayerHB>();
        healthPoint = GameObject.FindWithTag(healthTag).GetComponentInChildren<HealthPoint>();
    }

    public void Start()
    {
        Debug.Log("lives: " + lives);
        Health = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(MaxHealth);
        healthPoint.SetLives(lives);
    }

    public void Damage(int amount)
    {
        Health -= amount;
        healthBar.SetHealth(Health);
        if (Health <= 0)
        {
            Health = MaxHealth;
            lives--;
            healthBar.SetHealth(Health);
            healthPoint.ExplodeHeart();
        }

        if (lives <= 0)
        {
            //animator.SetTrigger("Death");
            gameManager.GameOver();
        }
    }

    public void GetLife()
    {
        Health = MaxHealth;
        healthBar.SetHealth(Health);
        if(lives == maxLives) return;
        lives++;
        Debug.Log("new lives: " + lives);
        healthPoint.AddHeart();
    }
}

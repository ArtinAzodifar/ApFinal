using System;
using System.Collections;
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
    private int lives;
    private GameManager gameManager;

    private void OnEnable()
    {
        CollectibleHealth.OnHealthCollected += GetLife;
    }

    private void OnDisable()
    {
        CollectibleHealth.OnHealthCollected -= GetLife;
    }

    public void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        healthBar = GameObject.FindWithTag(healthTag).GetComponentInChildren<PlayerHB>();
        healthPoint = GameObject.FindWithTag(healthTag).GetComponentInChildren<HealthPoint>();
    }

    public void Start()
    {
    }

    public void Damage(int amount)
    {
        Health -= amount;
        StartCoroutine(LockPlayer());
        healthBar.SetHealth(Health);
        if (Health <= 0)
        {
            animator.SetTrigger("Death");
            Health = MaxHealth;
            lives--;
            healthBar.SetHealth(Health);
            healthPoint.ExplodeHeart();
            StartCoroutine(GameOverCheck());
        }
        else
        {
            animator.SetTrigger("TakeHit");
        }
    }

    public void GetLife(GameObject player)
    {
        if (player != gameObject) return;
        Health = MaxHealth;
        healthBar.SetHealth(Health);
        if(lives == maxLives) return;
        lives++;
        healthPoint.AddHeart();
    }

    private IEnumerator GameOverCheck()
    {
        yield return new WaitForSeconds(1f);
        if (lives <= 0)
        {
            gameManager.GameOver();
        }
    }

    private IEnumerator LockPlayer()
    {
        gameObject.GetComponent<BaseControll>().setIsInDamage(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BaseControll>().setIsInDamage(false);
    }
    
    public void LoadHealth(int healthAmount, int livesAmount)
    {
        this.Health = healthAmount;
        this.lives = livesAmount;
        
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(this.Health);
        healthPoint.SetLives(this.lives);
    }

    public int getHealth()
    {
        return Health;
    }

    public int getLives()
    {
        return lives;
    }
}
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
    private int lives = 3;
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
        Health = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(MaxHealth);
        healthPoint.SetLives(lives);
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

    private void GameOverCheck() // at the end of death animation
    {
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
}

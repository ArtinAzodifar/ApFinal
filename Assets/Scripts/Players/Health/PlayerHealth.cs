using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour, Damagable
{
    private Animator animator;
    [SerializeField] private String healthTag;
    private PlayerHB healthBar;
    private HealthPoint healthPoint;
    [SerializeField] private int maxLives;
    [SerializeField] private int MaxHealth;
    private NetworkVariable<int> Health = new NetworkVariable<int>();
    private NetworkVariable<int> lives = new NetworkVariable<int>(3);
    private NetworkVariable<bool> isDying = new NetworkVariable<bool>(false);
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
        // This is fine, it just sets the slider's maximum possible value.
        healthBar.SetMaxHealth(MaxHealth);

        // Only initialize health and UI for a new game.
        // If a game is loaded, the SaveManager will handle setting the values.
        if (SaveManager.Instance == null || !SaveManager.Instance.IsGameLoaded)
        {
            if (gameManager.IsLocalMode() || IsServer)
            {
                Health.Value = MaxHealth;
            }
            // These lines are now correctly inside the IF block.
            healthBar.SetHealth(MaxHealth);
            healthPoint.SetLives(lives.Value);
        }
    }

    public void Damage(int amount)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        
        if (isDying.Value) return;
        Health.Value -= amount;
        StartCoroutine(LockPlayer());
        if (gameManager.IsLocalMode()) healthBar.SetHealth(Health.Value);
        else if (IsServer) updateHealthBarClientRpc(Health.Value);
        if (Health.Value <= 0)
        {
            isDying.Value = true;
            animator.SetTrigger("Death");
            Health.Value = MaxHealth;
            lives.Value--;

            if (gameManager.IsLocalMode()) healthBar.SetHealth(Health.Value);
            else if (IsServer) updateHealthBarClientRpc(Health.Value);

            if (gameManager.IsLocalMode()) healthPoint.ExplodeHeart();
            else if (IsServer) explodeHeartClientRpc();
            StartCoroutine(GameOverCheck());
        }
        else
        {
            animator.SetTrigger("TakeHit");
        }
    }

    public void GetLife(GameObject player)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (player != gameObject) return;
        Health.Value = MaxHealth;

        if (gameManager.IsLocalMode()) healthBar.SetHealth(Health.Value);
        else if (IsServer) updateHealthBarClientRpc(Health.Value);

        if (lives.Value == maxLives) return;
        lives.Value++;

        if (gameManager.IsLocalMode()) healthPoint.AddHeart();
        else if (IsServer) addHeartClientRpc();
    }

    private IEnumerator GameOverCheck()
    {
        yield return new WaitForSeconds(2.5f);
        if (lives.Value <= 0)
        {
            gameManager.GameOver();
        }
        isDying.Value = false;
    }

    private IEnumerator LockPlayer()
    {
        gameObject.GetComponent<BaseControll>().setIsInDamage(true);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BaseControll>().setIsInDamage(false);
    }
    
    public void LoadHealth(int healthAmount, int livesAmount)
    {
        // Assign the new values to the NetworkVariables
        Health.Value = healthAmount;
        lives.Value = livesAmount;
    
        // Set the UI using the direct parameter values, NOT by reading back from the NetworkVariable
        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(healthAmount); // Use healthAmount directly
        healthPoint.SetLives(livesAmount); // Use livesAmount directly
    }


    //getters
    public int getHealth()
    {
        return Health.Value;
    }
    public int getLives()
    {
        return lives.Value;
    }
    public bool IsDying()
    {
        return isDying.Value;
    }

    //setter
    public void setHealth(int amount)
    {
        Health.Value = amount;
    }


    //Rpc - for UI update in online mode
    [ClientRpc]
    private void updateHealthBarClientRpc(int health) { healthBar.SetHealth(health); }
    [ClientRpc]
    private void explodeHeartClientRpc() { healthPoint.ExplodeHeart(); }
    [ClientRpc]
    private void addHeartClientRpc() { healthPoint.AddHeart(); }
}


using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Damagable
{
    [SerializeField] private Animator animator;
    private int lives = 3;
    private int Health = 100;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Damage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 100;
            lives--;
        }

        if (lives <= 0)
        {
            //animator.SetTrigger("Death");
            Debug.Log("game over");
            //contrtoller: lose
        }
    }
}

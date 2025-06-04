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
            Animator animator = gameObject.GetComponent<Animator>();
            if (animator != null && HasTrigger(animator, "Death"))
            {
                animator.SetTrigger("Death");
                Destroy(gameObject, 1.8f);
            }
            else
            {
                Destroy(gameObject);   
            }
        }
    }
    
    private bool HasTrigger(Animator animator, string triggerName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
            {
                return true;
            }
        }
        return false;
    }
}
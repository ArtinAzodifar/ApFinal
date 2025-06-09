using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Damagable
{
    [SerializeField] private GameObject[] collectibles;
    [SerializeField] private int health;
    [SerializeField] private EnemyHB healthBar;
    public bool isDead = false;
    
    private SoundPlayer soundPlayer;
    [SerializeField] private AudioClip deathSoundClip;


    public void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(health);   
        }
        soundPlayer = GetComponent<SoundPlayer>();
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Animator animator = gameObject.GetComponent<Animator>();
            
            AudioSource.PlayClipAtPoint(deathSoundClip, transform.position, AudioController.Instance.sfxVolume);
            int random = UnityEngine.Random.Range(0, 100);
            Debug.Log(random);
            if (random < 25)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(collectibles[0], position, Quaternion.identity);//health
            } 
            else if (random < 50)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(collectibles[1], position, Quaternion.identity);//damage booster
            }
            else if (random < 60)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Instantiate(collectibles[2], position, Quaternion.identity);//max mana
            }
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
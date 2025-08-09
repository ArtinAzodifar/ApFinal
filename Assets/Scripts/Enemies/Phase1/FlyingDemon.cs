using System;
using UnityEditor.UIElements;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class FlyingDemon : NetworkBehaviour
{
    [SerializeField] private float scale;
    [SerializeField] private float shootTimer;
    [SerializeField] private float distance;
    [SerializeField] private float soundDistance;
    [SerializeField] private GameObject healthbar;
    private ObjectPooler fire;
    private GameObject melee;
    private GameObject range;
    private GameObject target;
    private Animator animator;
    private float timePast = 0;
    private bool canShoot = false;
    private AudioSource audioSource;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        transform.localScale = new Vector3(scale, scale, scale);
        fire = GameObject.FindWithTag("FireBallPool").GetComponent<ObjectPooler>();
        animator = GetComponent<Animator>();

        melee = GameObject.FindWithTag("Player1");
        range = GameObject.FindWithTag("Player2");

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
    }
    public void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        FindPlayer();

        if (canShoot && timePast >= shootTimer)
        {
            animator.SetTrigger("Attack");
            timePast = 0;
        }
        //cooldown
        else if (canShoot) timePast += Time.deltaTime;
    }

    private void FindPlayer()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        float meleeXDistance = transform.position.x - melee.transform.position.x;
        float leafXDistance = transform.position.x - range.transform.position.x;
        float meleeYDistance = transform.position.y - melee.transform.position.y;
        float leafYDistance = transform.position.y - range.transform.position.y;
        bool meleeInSight = Mathf.Abs(meleeXDistance) <= distance && Mathf.Abs(meleeYDistance) <= 2;
        bool leafInSight = Mathf.Abs(leafXDistance) <= distance && Mathf.Abs(leafYDistance) <= 2;
        if (meleeInSight && leafInSight)
        {
            canShoot = true;
            target = Mathf.Abs(meleeXDistance) <= Mathf.Abs(leafXDistance) ? melee : range;
            setDirection(target);
        }
        else if (meleeInSight)
        {
            canShoot = true;
            target = melee;
            setDirection(target);
        }
        else if (leafInSight)
        {
            canShoot = true;
            target = range;
            setDirection(target);
        }
        else
        {
            canShoot = false;
            target = null;
        }
    }

    private void Shoot() //is called in the middle of attack animation
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        float distance = transform.localScale.x > 0 ? -1f : 1f;
        Vector3 position = new Vector3(transform.position.x + distance, transform.position.y, 0);
        GameObject fireBall = fire.GetObject();
        fireBall.transform.position = position;
        fireBall.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void setDirection(GameObject target)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (target == null) return;
        
        float targetDistance = transform.position.x - target.transform.position.x;
        if (targetDistance < 0)
        {
            transform.localScale = new Vector3(-scale, scale, scale);
            healthbar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, scale);
            healthbar.transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
        }
    }


    //play sound when close
    public void FixedUpdate()
    {
        if (PlayerIsInTrigger()) audioSource.volume = AudioController.Instance.sfxVolume;
        else audioSource.volume = 0f;
    }

    private bool PlayerIsInTrigger()
    {
        return Vector2.Distance(transform.position, melee.transform.position) <= soundDistance || Vector2.Distance(transform.position, range.transform.position) <= soundDistance;
    }
}

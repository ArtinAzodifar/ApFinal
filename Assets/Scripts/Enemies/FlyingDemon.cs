using System;
using UnityEditor.UIElements;
using UnityEngine;

public class FlyingDemon : MonoBehaviour
{

    [SerializeField] private float shootTimer;
    [SerializeField] private float distance;
    [SerializeField] private GameObject healthbar;
    private ObjectPooler fire;
    private GameObject melee;
    private GameObject leaf;
    private GameObject target;
    private Animator animator;
    private float timePast = 0;
    private bool canShoot = false;

    public void Awake()
    {
        fire = GameObject.FindWithTag("FireBallPool").GetComponent<ObjectPooler>();
        animator = GetComponent<Animator>();
        melee = GameObject.FindWithTag("Player1");
        leaf = GameObject.FindWithTag("Player2");
    }
    public void Update()
    {
        FindPlayer();
        if (canShoot && timePast >= shootTimer)
        {
            animator.SetTrigger("Attack");
            timePast = 0;
        }
        else if (canShoot)
        {
            timePast += Time.deltaTime;
        }
    }

    private void FindPlayer()
    {
        float meleeDistance = transform.position.x - melee.transform.position.x;
        float leafDistance = transform.position.x - leaf.transform.position.x;
        if (Mathf.Abs(meleeDistance) <= distance || Mathf.Abs(leafDistance) <= distance)
        {
            canShoot = true;
            if (Mathf.Abs(meleeDistance) < Mathf.Abs(leafDistance)) //nazdik tare = melee
            {
                target = melee;
            }
            else
            {
                target = leaf;
            }
            setDirection(target);
        }
        else
        {
            canShoot = false;
            target = null;
        }
    }


    private void Shoot()//is called in the middle of attack animation
    {
        float distance = transform.localScale.x > 0 ? -1f : 1f;
        Vector3 position = new Vector3(transform.position.x + distance, transform.position.y, 0);
        GameObject fireBall = fire.GetObject();
        fireBall.transform.position = position;
        fireBall.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void setDirection(GameObject target)
    {
        if (target == null) return;
        float targetDistance = transform.position.x - target.transform.position.x;
        if (targetDistance < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            healthbar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            healthbar.transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
        }
    }
}

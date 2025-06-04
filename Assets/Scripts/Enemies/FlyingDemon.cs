using System;
using UnityEditor.UIElements;
using UnityEngine;

public class FlyingDemon : MonoBehaviour
{
    [SerializeField] private float scale;
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
        transform.localScale = new Vector3(scale, scale, scale);
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
        else if (canShoot)//cooldown
        {
            timePast += Time.deltaTime;
        }
    }

    private void FindPlayer()
    {
        float meleeXDistance = transform.position.x - melee.transform.position.x;
        float leafXDistance = transform.position.x - leaf.transform.position.x;
        float meleeYDistance = transform.position.y - melee.transform.position.y;
        float leafYDistance = transform.position.y - leaf.transform.position.y;
        bool meleeInSight = Mathf.Abs(meleeXDistance) <= distance && Mathf.Abs(meleeYDistance) <= 4;
        bool leafInSight = Mathf.Abs(leafXDistance) <= distance && Mathf.Abs(leafYDistance) <= 4;
        if (meleeInSight && leafInSight)
        {
            canShoot = true;
            target = Mathf.Abs(meleeXDistance) <= Mathf.Abs(leafXDistance) ? melee : leaf;
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
            target = leaf;
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
            transform.localScale = new Vector3(-scale, scale, scale);
            healthbar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            transform.localScale = new Vector3(scale, scale, scale);
            healthbar.transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
        }
    }
}

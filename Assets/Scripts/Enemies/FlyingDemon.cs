using UnityEngine;

public class FlyingDemon : MonoBehaviour
{

    [SerializeField] private float shootTimer;
    [SerializeField] private Transform melee;
    [SerializeField] private Transform leaf;
    [SerializeField] private GameObject fire;
    private Transform target;
    private Animator animator;
    private float timePast = 0;
    private bool canShoot = false;

    public void Awake()
    {
        animator = GetComponent<Animator>();
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
        float meleeDistance = transform.position.x - melee.position.x;
        float leafDistance = transform.position.x - leaf.position.x;
        if (Mathf.Abs(meleeDistance) <= 15 || Mathf.Abs(leafDistance) <= 15)
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
        float distance = transform.localScale.x > 0 ? -0.4f : 0.4f;
        Vector3 position = new Vector3(transform.position.x + distance, transform.position.y, 0);
        Instantiate(fire, position, transform.rotation);
    }

    public void setDirection(Transform target)
    {
        if (target == null) return;
        float targetDistance = transform.position.x - target.position.x;
        if (targetDistance < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseMovingEnemy : MonoBehaviour, MovingEnemy
{
    [SerializeField] protected float speed;
    [SerializeField] protected float distance;
    [SerializeField] protected float scale;
    [SerializeField] protected GameObject healthbar;
    protected GameObject melee;
    protected GameObject leaf;
    protected GameObject target;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool isChasing;
    protected bool inCoolDown = false;
    protected bool isMovingRight;

    //unity events:
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isChasing = false;
        isMovingRight = false;
        melee = GameObject.FindGameObjectWithTag("Player1");
        leaf = GameObject.FindGameObjectWithTag("Player2");
    }
    public virtual void Update()
    {
        animator.SetBool("Run", isChasing);
        FindPlayer();
        Chase();
    }

    public virtual void FindPlayer()
    {
        float meleeXDistance = transform.position.x - melee.transform.position.x;
        float leafXDistance = transform.position.x - leaf.transform.position.x;
        float meleeYDistance = transform.position.y - melee.transform.position.y;
        float leafYDistance = transform.position.y - leaf.transform.position.y;
        bool meleeInSight = Mathf.Abs(meleeXDistance) <= distance && Mathf.Abs(meleeYDistance) <= 4;
        bool leafInSight = Mathf.Abs(leafXDistance) <= distance && Mathf.Abs(leafYDistance) <= 4;
        if (meleeInSight && leafInSight)
        {
            isChasing = true;
            target = Mathf.Abs(meleeXDistance) <= Mathf.Abs(leafXDistance) ? melee : leaf;
            setDirection(target);
        }
        else if (meleeInSight)
        {
            isChasing = true;
            target = melee;
            setDirection(target);
        } 
        else if (leafInSight)
        {
            isChasing = true;
            target = leaf;
            setDirection(target);
        }
        else
        {
            isChasing = false;
            target = null;
        }
    }

    public virtual void Chase()
    {
        if (!isChasing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        setDirection(target);
        rb.linearVelocity = new Vector2((isMovingRight ? 1 : -1) * speed, 0);
    }

    public void setDirection(GameObject target)
    {
        if (target == null) return;
        float targetDistance = transform.position.x - target.transform.position.x;
        if (targetDistance < 0)
        {
            isMovingRight = true;
            transform.localScale = new Vector3(scale, Mathf.Abs(scale), Mathf.Abs(scale));
            if (healthbar != null)
            {
                healthbar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
        else
        {
            isMovingRight = false;
            transform.localScale = new Vector3(-scale, Mathf.Abs(scale), Mathf.Abs(scale));
            if (healthbar != null)
            {
                healthbar.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
    }
}
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
        if (isChasing || inCoolDown) return;
        ForceFindPlayer();
    }

    public virtual void ForceFindPlayer()
    {
        float meleeDistance = transform.position.x - melee.transform.position.x;
        float leafDistance = transform.position.x - leaf.transform.position.x;
        if (Mathf.Abs(meleeDistance) <= distance || Mathf.Abs(leafDistance) <= distance)
        {
            isChasing = true;
            if (Mathf.Abs(meleeDistance) < Mathf.Abs(leafDistance)) //nazdik tare = melee
            {
                target = melee;
            }
            else //nazdiktare leaf
            {
                target = leaf;
            }

            setDirection(target);
        }
    }

    public virtual void Chase()
    {
        if (!isChasing) return;
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
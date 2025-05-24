using UnityEngine;

public abstract class BaseMovingEnemy : MonoBehaviour, MovingEnemy
{
    [SerializeField] protected Transform melee;
    [SerializeField] protected Transform leaf;
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;
    // protected Animator animator;
    protected Transform target;
    protected bool isChasing;
    protected bool isMovingRight;

    //unity events:
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        isChasing = false;
        isMovingRight = false;
    }
    public virtual void Update()
    {
        // animator.SetBool("Run", isChasing);
        FindPlayer();
        Chase();
    }

    public void FindPlayer()
    {
        if (isChasing) return;

        float meleeDistance = transform.position.x - melee.position.x;
        float leafDistance = transform.position.x - leaf.position.x;
        if (Mathf.Abs(meleeDistance) <= 5 || Mathf.Abs(leafDistance) <= 5)
        {
            Debug.Log("found!");
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

    public void setDirection(Transform target)
    {
        if (target == null) return;
        float targetDistance = transform.position.x - target.position.x;
        if (targetDistance < 0)
        {
            isMovingRight = true;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            isMovingRight = false;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
    }
}
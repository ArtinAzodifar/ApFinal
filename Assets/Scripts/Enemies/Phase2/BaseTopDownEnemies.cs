using System;
using UnityEngine;

public class BaseTopDownEnemies : MonoBehaviour, MovingEnemy
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damageAmount;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    protected float _cooldownTimer = 0;
    
    protected Transform _meleePlayer;
    protected Transform _rangePlayer;
    protected Transform _closestPlayer;
    protected Rigidbody2D _rb;
    protected Vector2 _movement;
    protected Animator _animator;

    protected float _distance;
    protected float _distance1;
    protected float _distance2;
    
    void Start()
    {
        GameObject player1 = GameObject.FindWithTag("Player1");
        if (player1 != null) {
            _meleePlayer = player1.transform;
        }
        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null) {
            _rangePlayer = player2.transform;
        }
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        FindPlayer();
        Chase();
        Attack();
    }

    protected void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * (speed * Time.fixedDeltaTime));
    }

    public void FindPlayer()
    {
        if (_meleePlayer != null && _rangePlayer != null)
        {
            _distance1 = Vector2.Distance(transform.position, _meleePlayer.position);
            _distance2 = Vector2.Distance(transform.position, _rangePlayer.position);

            if (_distance1 <= _distance2)
            {
                _distance = _distance1;
                _closestPlayer = _meleePlayer;
            }
            else
            {
                _distance = _distance2;
                _closestPlayer = _rangePlayer;
            }
        }
        else if (_meleePlayer != null)
        {
            _distance = Vector2.Distance(transform.position, _meleePlayer.position);
            _closestPlayer = _meleePlayer;
        }
        else if (_rangePlayer != null)
        {
            _distance = Vector2.Distance(transform.position, _rangePlayer.position);
            _closestPlayer = _rangePlayer;
        }
    }

    public void Chase()
    {
        if (_distance <= detectionRange && _distance >= attackRange)
        {
            Vector2 direction = (_closestPlayer.position - transform.position).normalized;
            _movement = direction;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                _animator.SetFloat("XInput", direction.x > 0 ? 1 : -1);
                _animator.SetFloat("YInput", 0);
            }
            else
            {
                _animator.SetFloat("XInput", 0);
                _animator.SetFloat("YInput", direction.y > 0 ? 1 : -1);
            }

            _animator.SetFloat("Speed", _movement.magnitude);
        }
        else
        {
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);
        }
    }

    public virtual void Attack()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        if (_distance <= attackRange && _cooldownTimer <= 0)
        {
            Vector2 direction = (_closestPlayer.position - transform.position);
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                _animator.SetFloat("XInput", direction.x > 0 ? 1 : -1);
                _animator.SetFloat("YInput", 0);
            }
            else
            {
                _animator.SetFloat("XInput", 0);
                _animator.SetFloat("YInput", direction.y > 0 ? 1 : -1);
            }
            
            _animator.SetTrigger("Attack");
            _cooldownTimer = attackCooldown;
        }
    }

    public Vector2 getMovement()
    {
        return _movement;
    }
}

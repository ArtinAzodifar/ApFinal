using System;
using UnityEngine;

public class CloseRangeEnemy : MonoBehaviour, MovingEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    private float _cooldownTimer = 0;
    
    private Transform _meleePlayer;
    private Transform _rangePlayer;
    private Transform _closestPlayer;
    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Animator _animator;

    private float _distance;
    private float _distance1;
    private float _distance2;
    
    [SerializeField] private GameObject hitboxUp;
    [SerializeField] private GameObject hitboxDown;
    [SerializeField] private GameObject hitboxLeft;
    [SerializeField] private GameObject hitboxRight;
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

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        Chase();
        Attack();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * (speed * Time.fixedDeltaTime));
    }

    public void FindPlayer()
    {
        if (_meleePlayer)
        {
            _distance1 = Vector2.Distance(transform.position, _meleePlayer.position);
        }

        if (_rangePlayer)
        {
            _distance2 = Vector2.Distance(transform.position, _rangePlayer.position);
        }

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

    public void Attack()
    {
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

            _animator.SetBool("IsAttacking", true);

            _cooldownTimer = attackCooldown;
        } 
        else 
        {
            _animator.SetBool("IsAttacking", false);
        }

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }
    
    public void EnableUpHitbox() { hitboxUp.SetActive(true); }
    public void EnableDownHitbox() { hitboxDown.SetActive(true); }
    public void EnableLeftHitbox() { hitboxLeft.SetActive(true); }
    public void EnableRightHitbox() { hitboxRight.SetActive(true); }

    public void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }
}

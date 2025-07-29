using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
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
    private SpriteRenderer _spriteRenderer;

    private float _distance;
    private float _distance1;
    private float _distance2;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject spell;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float castCooldown;
    [SerializeField] private int numOfSpells;
    private float _spawnEnemyCooldownTimer = 0;
    private float _castCooldownTimer;

    void Start()
    {
        GameObject player1 = GameObject.FindWithTag("Player1");
        if (player1 != null)
        {
            _meleePlayer = player1.transform;
        }
        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null)
        {
            _rangePlayer = player2.transform;
        }

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _castCooldownTimer = castCooldown / 2;
    }

    void Update()
    {
        FindPlayer();
        Chase();
        Attack();
        RandomEnemySpawner();
        RandomSpellCasting();
    }

    private void FixedUpdate()
    {
        if (CanMove()) _rb.MovePosition(_rb.position + _movement * (speed * Time.fixedDeltaTime));
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
        if (CanMove() && _closestPlayer != null && _distance <= detectionRange && _distance > attackRange)
        {
            Vector2 direction = (_closestPlayer.position - transform.position).normalized;
            _movement = direction;

            if (direction.x > 0.01f)
            {
                _spriteRenderer.flipX = true;
            }
            else if (direction.x < -0.01f)
            {
                _spriteRenderer.flipX = false;
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
        if (_closestPlayer != null && _distance <= attackRange && _cooldownTimer <= 0.05)
        {
            
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);

            Vector2 direction = (_closestPlayer.position - transform.position);
            if (direction.x > 0.01f)
            {
                _spriteRenderer.flipX = false;
            }
            else if (direction.x < -0.01f)
            {
                _spriteRenderer.flipX = true;
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

    private void RandomEnemySpawner()
    {
        if (_distance <= spawnRadius && _spawnEnemyCooldownTimer <= 0.05)
        {
            
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);

            _animator.SetBool("IsSpawning", true);
            _spawnEnemyCooldownTimer = spawnCooldown;
            
            foreach (var e in enemies)
            {
                Vector3 randomPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
                Instantiate(e, randomPos, Quaternion.identity);
                Instantiate(spawn, randomPos, Quaternion.identity);
            }
        }
        else if (_spawnEnemyCooldownTimer > 0.05)
        {
            _animator.SetBool("IsSpawning", false);
        }

        if (_spawnEnemyCooldownTimer > 0f)
        {
            _spawnEnemyCooldownTimer -= Time.deltaTime;
        }
    }

    private void RandomSpellCasting()
    {
        if (_distance <= spawnRadius && _castCooldownTimer <= 0.05)
        {
            
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);

            _animator.SetBool("IsCasting", true);
            _castCooldownTimer = castCooldown;
            
            for (int i = 0; i < numOfSpells; i++)
            {
                Vector3 randomPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
                Instantiate(spell, randomPos, Quaternion.identity);
            }
        }
        else if (_castCooldownTimer > 0.05)
        {
            _animator.SetBool("IsCasting", false);
        }

        if (_castCooldownTimer > 0f)
        {
            _castCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private bool CanMove()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        bool isMovableState = currentState.IsName("Idle") || currentState.IsName("Walk");

        return isMovableState;
    }
}

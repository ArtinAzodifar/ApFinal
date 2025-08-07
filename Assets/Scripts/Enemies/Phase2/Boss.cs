using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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

    [SerializeField] private GameObject attackZone;
    [SerializeField] private LayerMask[] playerLayers;

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
        if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;
        if (_spawnEnemyCooldownTimer > 0f) _spawnEnemyCooldownTimer -= Time.deltaTime;
        if (_castCooldownTimer > 0f) _castCooldownTimer -= Time.deltaTime;

        FindPlayer();

        if (_closestPlayer == null) return;

        if (_distance <= attackRange && _cooldownTimer <= 0)
        {
            Attack();
            return;
        }

        if (_distance <= spawnRadius && _spawnEnemyCooldownTimer <= 0.1)
        {
            RandomEnemySpawner();
            return;
        }

        if (_distance <= spawnRadius && _castCooldownTimer <= 0)
        {
            RandomSpellCasting();
            return;
        }
        
        Chase();
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

            float scaleX = Mathf.Abs(transform.localScale.x);

            if (direction.x > -0.01f) // Facing Left
            {
                transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < 0.01f) // Facing Right
            {
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
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

            _animator.SetTrigger("Attack");
            _cooldownTimer = attackCooldown;
        }
    }
    
    private void ActiveCollider()
    {
        LayerMask combinedLayers = playerLayers[0] | playerLayers[1]; 
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRange, combinedLayers);

        foreach (Collider2D player in hitPlayers)
        {
            Damagable damagableComponent = player.GetComponent<Damagable>();
            if (damagableComponent != null)
            {
                damagableComponent.Damage(damageAmount);
            }

            BaseControll b = player.GetComponent<BaseControll>();
            if (b != null)
            {
                b.setKnockFromRight(player.transform.position.x <= transform.position.x);
                b.startKnock(300);
            }
        }
    }

    private void RandomEnemySpawner()
    {
        if (_distance <= spawnRadius && _spawnEnemyCooldownTimer <= 0.05)
        {
            
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);
            
            foreach (var e in enemies)
            {
                Vector3 randomPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
                Instantiate(e, randomPos, Quaternion.identity);
                Instantiate(spawn, randomPos, Quaternion.identity);
            }
            _animator.SetTrigger("Spawn");
            _spawnEnemyCooldownTimer = spawnCooldown;
        }
    }

    private void RandomSpellCasting()
    {
        if (_distance <= spawnRadius && _castCooldownTimer <= 0.05)
        {
            
            _movement = Vector2.zero;
            _animator.SetFloat("Speed", 0);
            
            for (int i = 0; i < numOfSpells; i++)
            {
                Vector3 randomPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
                Instantiate(spell, randomPos, Quaternion.identity);
            }
            
            _animator.SetTrigger("Cast");
            _castCooldownTimer = castCooldown;
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

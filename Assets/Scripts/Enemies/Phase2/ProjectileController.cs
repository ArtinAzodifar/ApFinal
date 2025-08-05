using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damageAmount;
    [SerializeField] private int timeToDestroy;

    private Vector2 _direction;
    private Rigidbody2D _rb;
    private Animator _animator;

    private Transform _meleePlayer;
    private Transform _rangePlayer;
    private Transform _closestPlayer;
    
    private float _distance;
    private float _distance1;
    private float _distance2;

    private float _timer;

    private bool _canMove = true;
    
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

        FindPlayer();
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_timer >= timeToDestroy)
        {
            _canMove = false;
            _animator.SetTrigger("Projection");
        }
        _timer += Time.deltaTime;
        
        Chase();
    }

    private void FixedUpdate()
    {
        if (_direction != Vector2.zero && _canMove)
        {
            _rb.MovePosition(_rb.position + _direction * (speed * Time.fixedDeltaTime));
        }
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
        _direction = (_closestPlayer.position - transform.position).normalized;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
           
            if (other.TryGetComponent<Damagable>(out Damagable damagable))
            {
                damagable.Damage(damageAmount);
            }

            _canMove = false;
            _animator.SetTrigger("Projection");
        }
    }

    public void DestroyOnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
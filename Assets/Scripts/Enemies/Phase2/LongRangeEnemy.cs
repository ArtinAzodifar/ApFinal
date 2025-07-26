using UnityEngine;

public class LongRangeEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float detectionRange;
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
}

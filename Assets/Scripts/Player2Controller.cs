using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    private Vector2 _movementInput;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject arrow;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private int _jumpCount;
    private const int MaxJump = 2;
    
    [SerializeField] private Transform groundCheckCollider;  
    private const float GroundCheckRadius = 0.2f;  
    [SerializeField] private LayerMask groundLayer;
    
    private bool _isGrounded;

    private bool _canShoot;
    private bool _canSuperShoot;

    public int superShootMana;
    private int _maxMana;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _jumpCount = 0;
        _canShoot = true;
        _maxMana = 10;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _jumpCount < MaxJump)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
            _rigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _jumpCount++;
            if (_jumpCount == 1) _animator.SetTrigger("Jump");
            else if (_jumpCount == 2) _animator.SetTrigger("DoubleJump");
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && _canShoot)
        {
            _canShoot = false;
            _animator.SetTrigger("Shoot");

            StartCoroutine(ShootArrowAfterDelay(0.6f));
            StartCoroutine(AllowShootingAgainAfter(1.2f));
        }
    }

    private IEnumerator ShootArrowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        float offsetX = Mathf.Abs(transform.localScale.x) * 0.5f;
        float offsetY = Mathf.Abs(transform.localScale.y) * 0.1f;

        Vector3 spawnPosition = transform.position + new Vector3(offsetX * direction, offsetY, 0f);
        GameObject newArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);

        Vector3 arrowScale = newArrow.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        newArrow.transform.localScale = arrowScale;
    }

    private IEnumerator AllowShootingAgainAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canShoot = true;
    }

    public void OnSuperShoot(InputAction.CallbackContext context)
    {
        if (context.performed && superShootMana >= _maxMana)
        {
            _animator.SetTrigger("SuperShoot");
            superShootMana -= _maxMana;
        }
    }

    void Update()
    {
        groundCheck();
        move();
    }

    private void move()  
    {
        Vector3 scale = transform.localScale;
        
        if (_movementInput.x > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        } else if (_movementInput.x < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        
        transform.localScale = scale;
        
        bool isRunning = Mathf.Abs(_movementInput.x) > 0.01f;  
        _animator.SetBool("IsRunning", isRunning);
        
        Vector2 move = new Vector2(_movementInput.x, 0) * (moveSpeed * Time.deltaTime);  
        transform.Translate(move);
    }

    void groundCheck()
    {
        _isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, GroundCheckRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.CompareTag("Player1"))
            {
                _isGrounded = true;
                break;
            }
        }

        if (_isGrounded && _rigidbody.linearVelocity.y <= 0.01f) //we need the second condition to prevent triple jump!
        {
            _jumpCount = 0;
        }
    }


}
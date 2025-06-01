using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : BaseControll
{
    private ObjectPooler ArrowPooler;
    
    private bool _canDoubleJump;

    private bool _canShoot;
    private bool _isCharging = false;
    private bool _isFullyCharged = false;
    private bool _canSuperShoot;

    public int superShootMana;
    private int _maxMana;
    
    public override void Awake()
    {
        base.Awake();
        ArrowPooler = GameObject.FindWithTag("ArrowPool").GetComponent<ObjectPooler>();
        _canShoot = true;
        _maxMana = 10;
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            _canDoubleJump = true;
            base.OnJump(context);
        } 
        else if (context.performed && _canDoubleJump)
        {
            _canDoubleJump = false;
            animator.SetTrigger("DoubleJump");//should be changed
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(GetJumpForce() * Vector2.up, ForceMode2D.Impulse);
        }
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("Shoot");
            
            StartCoroutine(AllowShootingAgainAfter(1f));
        }
    }

    private void Shoot()// is called in the middle of attack animation
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        float offsetX = Mathf.Abs(transform.localScale.x) * 0.5f;
        float offsetY = Mathf.Abs(transform.localScale.y) * 0.1f;

        Vector3 spawnPosition = transform.position + new Vector3(offsetX * direction, offsetY, 0f);
        GameObject newArrow = ArrowPooler.GetObject();
        newArrow.transform.position = spawnPosition;
        
        Vector3 arrowScale = newArrow.transform.localScale;
        arrowScale.x = Mathf.Abs(arrowScale.x) * direction;
        newArrow.transform.localScale = arrowScale;
    }

    public void OnSuperShoot(InputAction.CallbackContext context)
    {
        if (context.performed && superShootMana >= _maxMana)
        {
            animator.SetTrigger("SuperShoot");
            superShootMana -= _maxMana;
        }
    }
}
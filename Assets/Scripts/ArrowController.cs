using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Animator animator;
    private bool canMove = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            transform.Translate(Vector2.right * (direction * speed * Time.deltaTime));
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        canMove = false;
        animator.SetTrigger("Arrow-hit");
        StartCoroutine(ArrowHitCooldown(0.7f));
    }

    private IEnumerator ArrowHitCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        Destroy(gameObject);
    }
}
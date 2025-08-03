using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class Player1controll : BaseControll
{
    [SerializeField] private float dashForce = 15f;
    private TrailRenderer tr;
    private bool isDashing = false;
    private bool canDash = true;

    //inputs:
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!gameManager.IsLocalMode() && !IsOwner) return;
        if (context.performed && canDash && IsRunning() && !GetComponent<Player1Attack>().IsAttacking() && !gameObject.GetComponent<PlayerHealth>().IsDying())
        {
            StartCoroutine(Dash());
            animator.Play("Dash");
        }
    }

    //unity events:
    public override void Awake()
    {
        base.Awake();
        tr = GetComponent<TrailRenderer>();
    }
    public override void Update()
    {
        if (!gameManager.IsLocalMode() && !IsOwner) return;
        if (isDashing)  return;
        base.Update();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float tempG = rb.gravityScale;
        rb.gravityScale = 0;
        tr.emitting = true;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
        yield return new WaitForSeconds(0.25f);

        tr.emitting = false;
        rb.gravityScale = tempG;
        isDashing = false;
        rb.linearVelocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }
}

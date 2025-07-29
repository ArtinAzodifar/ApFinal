using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;

    private SpriteRenderer spriteRender;
    private Material mat;

    private Coroutine _FlashDamageCoroutine;
    private void Awake()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        mat = spriteRender.material;
    }

    public void CallDamageFlash()
    {
        _FlashDamageCoroutine = StartCoroutine(DamageFlasher());
    }
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(0.8f, _flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    private void SetFlashColor()
    {
        mat.SetColor("_FlashColor", flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        mat.SetFloat("_FlashAmount", amount);
    }
}

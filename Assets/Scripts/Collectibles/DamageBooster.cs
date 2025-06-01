using System;
using UnityEngine;

public class DamageBooster : MonoBehaviour
{
    public static event Action<GameObject, int, float> OnDamageBoost;
    private bool isCollected = false;
    private float damageBoosterTimer = 5f;
    private int damageBoosterAmount = 1;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            OnDamageBoost?.Invoke(other.gameObject, damageBoosterAmount, damageBoosterTimer);
            Destroy(gameObject);
        }
    }

}

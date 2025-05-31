using System;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    public static event Action<GameObject> OnHealthCollected;
    private bool isCollected = false;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            OnHealthCollected?.Invoke(other.gameObject);
            Destroy(gameObject);
        }
    }
}

using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public static event Action<GameObject> KeyCollected;
    private bool isCollected = false;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            KeyCollected?.Invoke(other.gameObject);
            Destroy(gameObject);
        }
    }
}

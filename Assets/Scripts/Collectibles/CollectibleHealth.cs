using System;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    private bool isCollected = false;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            other.gameObject.GetComponent<PlayerHealth>().GetLife();
            Destroy(gameObject);
        }
    }
}

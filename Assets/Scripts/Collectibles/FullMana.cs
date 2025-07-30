using System;
using UnityEngine;

public class FullMana : MonoBehaviour
{
    public static event Action<GameObject> ManaFill;
    private bool isCollected = false;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            ManaFill?.Invoke(other.gameObject);
            if(GetComponent<PersistentObject>() != null)    GetComponent<PersistentObject>().OnProcessed();
            Destroy(gameObject);
        }
    }
}

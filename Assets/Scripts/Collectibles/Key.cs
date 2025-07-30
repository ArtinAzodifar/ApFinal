using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public static event Action KeyCollected;
    private bool isCollected = false;
    private GameManager gameManager = GameManager.Instance;

    public void Start()
    {
        if (gameManager.GetKey1())
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected)    return;
            isCollected = true;
            KeyCollected?.Invoke();
            if(GetComponent<PersistentObject>() != null)    GetComponent<PersistentObject>().OnProcessed();
            Destroy(gameObject);
        }
    }
}

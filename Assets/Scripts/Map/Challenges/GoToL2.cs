using System;
using UnityEngine;

public class GoToL2 : MonoBehaviour
{
    private GameManager gameManager = GameManager.Instance;
    private bool keyFound = false;

    public void OnEnable()
    {
        Key.KeyCollected += FoundKey;
    }

    public void OnDisable()
    {
        Key.KeyCollected -= FoundKey;
    }

    public void FoundKey()
    {
        keyFound = true;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2")) && keyFound)
        {
            gameManager.Level2();
            Debug.Log(other.gameObject.name);
        }
    }
}

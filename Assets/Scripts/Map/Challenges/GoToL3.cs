using System;
using UnityEngine;

public class GoToL3 : MonoBehaviour
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
            gameManager.Level3();
            Debug.Log(other.gameObject.name);
        }
    }
}
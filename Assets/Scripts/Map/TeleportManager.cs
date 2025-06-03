using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Player1controll player1;
    [SerializeField] private Player2Controller player2;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            DestroyAllChildren();
            player1.Teleport(destination.position);
        }

        if (other.CompareTag("Player2"))
        {
            DestroyAllChildren();
            player2.Teleport(destination.position);
        }
    }
    
    void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

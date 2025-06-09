using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private Player1controll player1;
    private Player2Controller player2;

    void Start()
    {
        GameObject player1Tag = GameObject.FindGameObjectWithTag("Player1");
        player1 = player1Tag.GetComponent<Player1controll>();
        GameObject player2Tag = GameObject.FindGameObjectWithTag("Player2");
        player2 = player2Tag.GetComponent<Player2Controller>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player1"))
        {
            player1.Teleport(destination.position);
        }

        if (other.CompareTag("Player2"))
        {
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

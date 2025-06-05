using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private Player1controll player1;
    private Player2Controller player2;

    void Start()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Player1"))
            {
                player1 = obj.gameObject.GetComponent<Player1controll>();
            } 
            if (obj.layer == LayerMask.NameToLayer("Player2"))
            {
                player2 = obj.gameObject.GetComponent<Player2Controller>();
            }
        }
    }
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

using UnityEngine;

public class TeleportException : MonoBehaviour
{
    [SerializeField] private Transform destination1;
    [SerializeField] private Transform destination2;
    [SerializeField] private Player1controll player1;
    [SerializeField] private Player2Controller player2;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            DestroyAllChildren();
            player1.Teleport(destination1.position);
        }

        if (other.CompareTag("Player2"))
        {
            DestroyAllChildren();
            player2.Teleport(destination2.position);
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

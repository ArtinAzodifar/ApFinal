using Unity.Netcode;
using UnityEngine;

public class GoToL2 : NetworkBehaviour
{
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if ((other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2")) && gameManager.GetKey())
        {
            gameManager.setKey(false);
            gameManager.Level2();
            Debug.Log(other.gameObject.name);
            
        }
    }
}

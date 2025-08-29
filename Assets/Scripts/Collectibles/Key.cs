using System;
using UnityEngine;
using Unity.Netcode;

public class Key : NetworkBehaviour
{
    public static event Action KeyCollected;
    private bool isCollected = false;
    private GameManager gameManager = GameManager.Instance;

    public void Start()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        if (gameManager.GetKey())
        {
            //local
            if (GameManager.Instance.IsLocalMode())
            {
                Destroy(gameObject);
                return;
            }

            //online
            if (GetComponent<NetworkObject>() != null)
            {
                if ((bool)GetComponent<NetworkObject>().IsSceneObject) DestroyClientRpc();
                GetComponent<NetworkObject>().Despawn();
            }
            else DestroyClientRpc();
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected) return;
            isCollected = true;
            KeyCollected?.Invoke();
            if (GetComponent<PersistentObject>() != null) GetComponent<PersistentObject>().OnProcessed();

            //local
            if (GameManager.Instance.IsLocalMode())
            {
                Destroy(gameObject);
                return;
            }

            //online
            if ((bool)GetComponent<NetworkObject>().IsSceneObject) DestroyClientRpc();
            GetComponent<NetworkObject>().Despawn();
        }
    }

    [ClientRpc]
    private void DestroyClientRpc() { Destroy(gameObject); }
}

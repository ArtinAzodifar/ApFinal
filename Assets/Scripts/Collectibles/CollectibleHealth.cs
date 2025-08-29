using System;
using UnityEngine;
using Unity.Netcode;

public class CollectibleHealth : NetworkBehaviour
{
    public static event Action<GameObject> OnHealthCollected;
    private bool isCollected = false;
    private GameObject melee;
    private GameObject range;

    public void Start()
    {
        melee = GameObject.FindWithTag("Player1");
        range = GameObject.FindWithTag("Player2");
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (isCollected) return;
            isCollected = true;

            int c = other.gameObject.CompareTag("Player1") ? 1 : 2;

            if (GameManager.Instance.IsLocalMode()) OnHealthCollected?.Invoke(other.gameObject);
            else invokeClientRpc(c);

            if (GameManager.Instance.IsLocalMode() && GetComponent<PersistentObject>() != null) GetComponent<PersistentObject>().OnProcessed();

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

    [ClientRpc]
    private void DestroyClientRpc() { Destroy(gameObject); }
    [ClientRpc]
    private void invokeClientRpc(int c) { OnHealthCollected?.Invoke(c == 1 ? melee : range); }
}

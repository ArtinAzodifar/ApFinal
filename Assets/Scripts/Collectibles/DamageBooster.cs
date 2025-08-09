using System;
using UnityEngine;
using Unity.Netcode;

public class DamageBooster : NetworkBehaviour
{
    public static event Action<GameObject, int, float> OnDamageBoost;
    private bool isCollected = false;
    private float damageBoosterTimer = 5f;
    private int damageBoosterAmount = 1;

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

            if (GameManager.Instance.IsLocalMode()) OnDamageBoost?.Invoke(other.gameObject, damageBoosterAmount, damageBoosterTimer);
            else invokeClientRpc(c);
            
            if (GetComponent<PersistentObject>() != null) GetComponent<PersistentObject>().OnProcessed();

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
    private void invokeClientRpc(int c) { OnDamageBoost?.Invoke(c == 1 ? melee : range, damageBoosterAmount, damageBoosterTimer); }

}

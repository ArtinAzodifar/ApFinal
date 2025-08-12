using System;
using UnityEngine;
using Unity.Netcode;

public class TargetDetection : NetworkBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform keyPosition;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        if (other.gameObject.CompareTag("ArrowPool"))
        {
            var key = Instantiate(keyPrefab, keyPosition.position, keyPosition.rotation);

            if (!GameManager.Instance.IsLocalMode()) key.GetComponent<NetworkObject>().Spawn();
        }
    }
}

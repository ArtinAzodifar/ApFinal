using UnityEngine;
using Unity.Netcode;

public class DestroyOnAnimationEnd : NetworkBehaviour
{
    public void DestroySelf()
    {
        if (GameManager.Instance.IsLocalMode()) Destroy(gameObject);
        else if (IsServer) GetComponent<NetworkObject>().Despawn();
    }
}
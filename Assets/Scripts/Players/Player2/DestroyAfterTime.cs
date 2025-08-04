using UnityEngine;
using Unity.Netcode;

public class DestroyAfterTime : NetworkBehaviour
{
    public float lifeTime = 2f;
    private float timer;

    void Update()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            if (!GameManager.Instance.IsLocalMode())
                GetComponent<NetworkObject>().Despawn();
            else
                Destroy(gameObject);
        }
    }
}

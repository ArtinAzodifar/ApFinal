using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] private List<PrefabData> prefabDatas;

    public void Start()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;
        foreach (PrefabData data in prefabDatas)
        {
            foreach (Transform pos in data.positions)
            {
                var obj = Instantiate(data.prefab, pos.position, pos.rotation);

                //if online
                var netObj = obj.GetComponent<NetworkObject>();
                if (!GameManager.Instance.IsLocalMode() && IsServer && netObj != null) netObj.Spawn();
            }
        }
    }
}

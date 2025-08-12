using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] private List<PrefabData> prefabDatas;

    private void Awake()
    {
        GeneratePersistentIds();
    }

    private void GeneratePersistentIds()
    {
        int positionCounter = 0;
        foreach (PrefabData data in prefabDatas)
        {
            foreach (SpawnPointData spawnPoint in data.spawnPoints)
            {
                if (spawnPoint.spawnTransform != null)
                {
                    string positionHash = spawnPoint.spawnTransform.position.GetHashCode().ToString();
                    spawnPoint.uniqueIdInChunk = $"pos_{positionCounter}_{positionHash}";
                    positionCounter++;
                }
            }
        }
    }

    public void Start()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        foreach (PrefabData data in prefabDatas)
        {
            foreach (SpawnPointData spawnPoint in data.spawnPoints)
            {
                if (spawnPoint.spawnTransform == null) continue;

                var obj = Instantiate(data.prefab, spawnPoint.spawnTransform.position, spawnPoint.spawnTransform.rotation, transform);
                var persistentComp = obj.GetComponent<PersistentObject>();
                if (persistentComp != null)
                {
                    persistentComp.uniqueIdInChunk = spawnPoint.uniqueIdInChunk;
                }

                var netObj = obj.GetComponent<NetworkObject>();
                if (!GameManager.Instance.IsLocalMode() && IsServer && netObj != null)
                {
                    netObj.Spawn();
                }
            }
        }
    }
}
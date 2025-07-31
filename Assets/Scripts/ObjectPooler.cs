using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectPooler : NetworkBehaviour
{
    [SerializeField] private GameObject prefab;
    private int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void Start()
    {
        if (gameManager.IsLocalMode() || IsServer) InitPool();
        //else gameObject.SetActive(false);
    }

    private void InitPool()
    {
        for (int i = 0; i < poolSize; i++) CreateObject();
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            //we have an available object
            if (!obj.activeInHierarchy)
            {
                setActiveObject(obj, true);
                return obj;
            }
        }
        if (gameManager.IsLocalMode() || IsServer)
        {
            //we dont have any available object
            GameObject obj2 = CreateObject();
            setActiveObject(obj2, true);
            return obj2;
        }
        return null;
    }

    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab);

        //spawn in online mode
        if (!gameManager.IsLocalMode() && IsServer)
        {
            var netObj = obj.GetComponent<NetworkObject>();
            if (netObj != null) netObj.Spawn();
        }

        setActiveObject(obj, false);
        pool.Add(obj);
        return obj;
    }

    private void setActiveObject(GameObject obj, bool active)
    {
        if (GameManager.Instance.IsLocalMode()) obj.SetActive(active);
        else setActiveObjectClientRpc(obj.GetComponent<NetworkObject>().NetworkObjectId, active);
    }
    [ClientRpc]
    private void setActiveObjectClientRpc(ulong netID, bool active)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netID, out var netObj)) // gpt =)
        {
            netObj.gameObject.SetActive(active);
        }
    }
}

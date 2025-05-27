using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private int poolSize = 10;
    private List<GameObject> pool;

    public void Start()
    {
        pool = new List<GameObject>();
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateObject();
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject obj2 = CreateObject();
        obj2.SetActive(true);
        return obj2;
    }

    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}

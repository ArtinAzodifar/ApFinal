using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnPointData
{
    public Transform spawnTransform;
    public string uniqueIdInChunk;
}

[Serializable]
public class PrefabData
{
    public GameObject prefab;
    public List<SpawnPointData> spawnPoints = new();
}
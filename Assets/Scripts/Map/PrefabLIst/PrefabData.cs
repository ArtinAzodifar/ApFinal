using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PrefabData
{
    public GameObject prefab;
    public List<Transform> positions = new();
}

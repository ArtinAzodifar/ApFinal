using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WinCondition : NetworkBehaviour
{
    [SerializeField] private GameObject boss;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Update()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;
        if (boss == null) gameManager.Win();
    }
}

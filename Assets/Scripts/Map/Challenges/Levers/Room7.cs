using System;
using UnityEngine;
using Unity.Netcode;

public class Room7 : NetworkBehaviour, LeverToggle
{
    [SerializeField] private Sprite[] sprites;
    private NetworkVariable<int> currentSprite = new NetworkVariable<int>(0);
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        spriteRenderer.sprite = sprites[0];
    }

    public void Toggle()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (++currentSprite.Value >= 7) currentSprite.Value = 0;

        if (gameManager.IsLocalMode()) spriteRenderer.sprite = sprites[currentSprite.Value];
        else changeSpriteClientRpc(currentSprite.Value);
    }
    [ClientRpc]
    private void changeSpriteClientRpc(int value)
    {
        spriteRenderer.sprite = sprites[value];
    }

    public int GetSprite()
    {
        return currentSprite.Value;
    }
}

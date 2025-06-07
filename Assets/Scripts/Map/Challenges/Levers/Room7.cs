using System;
using UnityEngine;

public class Room7 : MonoBehaviour, LeverToggle
{
    [SerializeField] private Sprite[] sprites;
    private int currentSprite;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = 0;
    }

    public void Start()
    {
        spriteRenderer.sprite = sprites[currentSprite];
    }

    public void Toggle()
    {
        if (++currentSprite >= 7)
        {
            currentSprite = 0;
        }
        spriteRenderer.sprite = sprites[currentSprite];
    }

    public int GetSprite()
    {
        return currentSprite;
    }
}

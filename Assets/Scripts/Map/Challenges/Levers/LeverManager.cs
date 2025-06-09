using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class LeverManager : MonoBehaviour
{
    public static event Action<GameObject> doorOpen;
    [SerializeField] private Room7[] levers;
    [SerializeField] private int[] correctSprites;
    [SerializeField] private GameObject door;
    private bool opened = false;

    public void Update()
    {
        if (levers[0].GetSprite() == correctSprites[0]-1 && correctSprites[1]-1 == levers[1].GetSprite() &&
            correctSprites[2]-1 == levers[2].GetSprite() && !opened)
        {
            doorOpen?.Invoke(door);
            opened = true;
        }
    }
}

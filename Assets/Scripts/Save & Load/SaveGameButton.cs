using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveGameButton : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveButton()
    {
        Debug.Log("Saved2");
        SaveManager.Instance.SaveGame();
    }
}
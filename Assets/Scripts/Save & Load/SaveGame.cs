using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public void SaveButton()
    {
        Debug.Log("Saved2");
        SaveManager.Instance.SaveGame();
    }
}
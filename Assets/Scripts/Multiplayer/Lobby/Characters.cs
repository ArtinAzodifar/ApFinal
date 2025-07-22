using System;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    [SerializeField] private int charID;
    [SerializeField] private TMP_Text owner;
    private ulong clientID;
    private string ownerEmail;
    private bool isSelected;

    public void Awake()
    {
        owner.text = "[NOT SELECTED]";
    }

    public void updateState()
    {
        if (isSelected == true)
        {
            owner.text = ownerEmail;
        }
        else
        {
            owner.text = "[NOT SELECTED]";
        }
    }

    //getters
    public int CharacterID()
    {
        return charID;
    }
    public ulong ClientID()
    {
        return clientID;
    }
    public bool IsSelected()
    {
        return isSelected;
    }

    //setters
    public void SetSelected(bool selected, ulong id, string ownerEmail)
    {
        isSelected = selected;
        clientID = id;
        this.ownerEmail = ownerEmail;
        updateState();
    }
}

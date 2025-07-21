using System;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine.UI;

public class Character : NetworkBehaviour
{
    public GameObject characterPrefab;
    [SerializeField] private int charID;
    [SerializeField] private TMP_Text owner;
    private NetworkVariable<ulong> clientID = new NetworkVariable<ulong>();
    private NetworkVariable<FixedString64Bytes> ownerEmail = new NetworkVariable<FixedString64Bytes>();
    private NetworkVariable<bool> isSelected = new NetworkVariable<bool>();

    public void Awake()
    {
        owner.text = "[NOT SELECTED]";
    }

    public void updateState()
    {
        if (isSelected.Value == true)
        {
            owner.text = ownerEmail.Value.ToString();
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
        return clientID.Value;
    }
    public bool IsSelected()
    {
        return isSelected.Value;
    }
    
    //setters
    public void SetSelected(bool selected, ulong id, string ownerEmail)
    {
        isSelected.Value = selected;
        clientID.Value = id;
        this.ownerEmail.Value = new FixedString64Bytes(ownerEmail);
    }
}

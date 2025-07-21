using System;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private int charID;
    [SerializeField] private TMP_Text owner;
    private NetworkVariable<ulong> clientID = new NetworkVariable<ulong>();
    private NetworkVariable<FixedString64Bytes> ownerEmail;
    private NetworkVariable<bool> isSelected = new NetworkVariable<bool>(false);

    public void Awake()
    {
        owner.text = "[NOT SELECTED]";
    }

    public void updateState()
    {
        if (isSelected.Value == true)
        {
            owner.text = ownerEmail.ToString();
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
        this.ownerEmail.Value = ownerEmail;
    }
}

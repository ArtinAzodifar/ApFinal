using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

public struct CharacterSelectState : INetworkSerializable, IEquatable<CharacterSelectState>
{
    //maps the each client to the character which is selected
    public ulong clientID;
    public int characterID;
    public FixedString64Bytes email;

    public CharacterSelectState(ulong clientID, int characterID = -1, FixedString64Bytes email = default)
    {
        this.clientID = clientID;
        this.characterID = characterID;
        this.email = email;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref characterID);
        serializer.SerializeValue(ref email);
    }

    public bool Equals(CharacterSelectState other)
    {
        return clientID == other.clientID && characterID == other.characterID;
    }
}
using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ProjectWilson.Gameplay.GameplayObjects
{
    /// <summary>
    /// NetworkBehaviour containing only one NetworkVariableString which represents this object's name.
    /// </summary>
    public class NetworkNameState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<FixedPlayerName> Name = new NetworkVariable<FixedPlayerName>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    }

    /// <summary>
    /// Wrapping FixedString so that if we want to change player name max size in the future, we only do it once here
    /// </summary>
    public struct FixedPlayerName : INetworkSerializable
    {
        FixedString4096Bytes _Name;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _Name);
        }

        public override string ToString()
        {
            return _Name.Value.ToString();
        }

        public static implicit operator string(FixedPlayerName s) => s.ToString();
        public static implicit operator FixedPlayerName(string s)
        { Debug.LogWarning($"[test] kkkkkkkkkkkk s = {s}");
            return new FixedPlayerName() { _Name = new FixedString32Bytes(s) };
        }
    }
}

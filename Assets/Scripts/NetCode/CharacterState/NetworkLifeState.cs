using System;
using Unity.Netcode;
using UnityEngine;

namespace ProjectWilson.Gameplay.GameplayObjects
{
    /// <summary>
    /// MonoBehaviour containing only one NetworkVariable of type LifeState which represents this object's life state.
    /// </summary>
    public enum LifeState
    {
        Alive,
        Fainted,
        Dead,
    }

    public class NetworkLifeState : NetworkBehaviour
    {
        public NetworkVariable<LifeState> Life = new NetworkVariable<LifeState>(LifeState.Alive);
    }
}

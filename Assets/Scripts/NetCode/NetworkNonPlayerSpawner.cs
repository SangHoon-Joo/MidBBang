using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity​Engine.Assertions;

namespace ProjectWilson
{
    public class NetworkNonPlayerSpawner : NetworkObjectSpawner
    {
        public NetworkNonPlayer.Side Side;

        public override NetworkObject SpawnNetworkObject()
        {
            Debug.LogWarning($"[test] NetworkNonPlayerSpawner : SpawnNetworkObject() : Side = {Side}");
            var networkObject = base.SpawnNetworkObject();
            
            NetworkNonPlayer networkNonPlayer = networkObject.GetComponent<NetworkNonPlayer>();
            //networkNonPlayer.CurrentSide = Side;

            return networkObject;
        }
    }
}

using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity​Engine.Assertions;
using System.Threading.Tasks;

namespace ProjectWilson
{
    public class NetworkNonPlayerSpawnerRepeat : NetworkObjectSpawner
    {
        private int _WaveCount = 3;
        private int _IndividualCount = 3;
        private readonly int _IndividualInterverTimeMilliSec = 1000;
        private readonly int _WaveInterverTimeMilliSec = 45000;
        public override NetworkObject SpawnNetworkObject()
        {
            return base.SpawnNetworkObject();
        }

        public async void SpawnNetworkObjectWaveRepeat()
        {
            for(int i=0; i<_WaveCount; i++)
            {
                SpawnNetworkObjectIndividualRepeat();
                await Task.Delay(_WaveInterverTimeMilliSec);
            }
        }

        public async void SpawnNetworkObjectIndividualRepeat()
        {
            for(int i=0; i<_IndividualCount; i++)
            {
                SpawnNetworkObject();
                await Task.Delay(_IndividualInterverTimeMilliSec);
            }
        }
    }
}

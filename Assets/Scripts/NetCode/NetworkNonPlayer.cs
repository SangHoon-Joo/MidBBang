using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using ProjectWilson.Navigation;
using System.Threading.Tasks;

namespace ProjectWilson
{
    public class NetworkNonPlayer : NetworkCharacter
    {
        public enum Side
        {
            Red = 0,
            Blue,
            None,
            Max
        }

        [SerializeField]
        private NonPlayerAI _NonPlayerAI;
        private Side _Side;
        public Side CurrentSide
        {
            get => _Side;
            set {
                if (!_InitializedSide) { _InitializedSide = true; }
                _Side = value;
            }
        }
        
        private bool _InitializedSide;

        private void awake()
        {
            Assert.IsNotNull<NonPlayerAI>(_NonPlayerAI);
        }
        public override async void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                int retryCount = 0;
                for(;;)
                {
                    if(_InitializedSide)
                        break;

                    retryCount++;
                    if(retryCount >= 50)
                    {
                        Debug.LogError("_InitializedSide is false!!!");
                        return;
                    }
                    await Task.Delay(100);
                }
                _NonPlayerAI.Init(_Side, _TableData.Speed, _TableData.Sight);
            }                
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                
            }
        }

        protected override void Attack(float range, int amount)
        {
            base.Attack(range, amount);
        }
    }
}
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
        [SerializeField]
        private NonPlayerAI _NonPlayerAI;
        
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
                _NonPlayerAI.Init(CurrentSide, _TableData.Speed, _TableData.Sight, _TableData.AttackRange, _TableData.AttackDamage);
            }                
        }

        protected override void Attack(float range, int amount)
        {
            base.Attack(range, amount);
        }
    }
}
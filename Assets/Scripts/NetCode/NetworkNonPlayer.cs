using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;


namespace ProjectWilson
{
    public class NetworkNonPlayer : NetworkCharacter
    {
        [SerializeField]
        private NavMeshAgent _NavMeshAgent;

        private void awake()
        {
            Assert.IsNotNull<NavMeshAgent>(_NavMeshAgent);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                //_CharacterMovementController.runSpeed = _TableData.Speed;
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
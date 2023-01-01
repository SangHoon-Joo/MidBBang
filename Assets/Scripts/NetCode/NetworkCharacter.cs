using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectWilson.Gameplay.GameplayObjects;

namespace ProjectWilson
{
    public class NetworkCharacter : NetworkActor
    {
		public float Sight { protected set;	get; }
		
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsOwner)
            {
                Sight = _TableData.Sight;
            }
        }

        protected virtual void Attack(float range, int amount)
        {
            NetworkCharacter targetNetworkCharacter = DetectNearbyCharacter(range);
            if(targetNetworkCharacter == null)
                return;
            
            targetNetworkCharacter.ReceiveHP(-amount);
        }

        public NetworkCharacter DetectNearbyCharacter(float range)
        {
            RaycastHit hitInfo;
            if(DetectNearbyEntity(true, true, Collider, range, out hitInfo))
                return hitInfo.collider.GetComponent<NetworkCharacter>();
            
            return null;
        }

        public bool DetectNearbyEntity(bool wantPC, bool wantNPC, Collider attacker, float range, out RaycastHit hitInfo)
        {
            int mask = 0;
            if (wantPC)
                mask |= (1 << LayerMask.NameToLayer("PC"));
            if (wantNPC)
                mask |= (1 << LayerMask.NameToLayer("NPC"));

            if(Physics.BoxCast(attacker.transform.position, attacker.bounds.extents, attacker.transform.forward, out hitInfo, Quaternion.identity, range, mask))
                return true;

            return false;
        }
    }
}
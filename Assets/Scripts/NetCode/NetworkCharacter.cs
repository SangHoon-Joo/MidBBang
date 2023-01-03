using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectWilson.Gameplay.GameplayObjects;

namespace ProjectWilson
{
    public class NetworkCharacter : NetworkActor
    {
        public enum Side
        {
            Red = 0,
            Blue,
            None,
            Max
        }

        protected Side _Side;
        public Side CurrentSide
        {
            get => _Side;
            set {
                if (!_InitializedSide) { _InitializedSide = true; }
                _Side = value;
            }
        }
        protected bool _InitializedSide;

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
            NetworkCharacter targetNetworkCharacter = DetectForwardCharacter(range);
            if(targetNetworkCharacter == null || targetNetworkCharacter.CurrentSide == CurrentSide)
                return;
            
            targetNetworkCharacter.ReceiveHP(-amount);
        }

        public NetworkCharacter DetectForwardCharacter(float range)
        {
            RaycastHit hitInfo;
            if(DetectForwardEntity(true, true, Collider, range, out hitInfo))
                return hitInfo.collider.GetComponent<NetworkCharacter>();
            
            return null;
        }

        public bool DetectForwardEntity(bool wantPC, bool wantNPC, Collider attacker, float range, out RaycastHit hitInfo)
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
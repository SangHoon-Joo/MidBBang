using System.Collections;
using ProjectWilson.Actions;
using ProjectWilson.Extensions;
using ProjectWilson.Lookups;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using ProjectWilson.Navigation;

namespace ProjectWilson
{
	public class NonPlayerAI : SuperStateMachine
    {
        public enum AIState
        {
            Idle = 0,
            Move,
            Chase,
            Attack,
            Max
        }

        [SerializeField]
        private NavMeshAgent _NavMeshAgent;
        private DynamicNavPath _NavPath;
        [SerializeField]
        private CapsuleCollider _Collider;
        private float _Speed = 2f;
        public float RotationSpeed = 10f;
        private NetworkNonPlayer.Side _Side;
        private readonly Vector3 _FinalRedSideDestination = new Vector3 (25f, 0f, 25f);
        private readonly Vector3 _FinalBlueSideDestination = new Vector3 (-25f, 0f, -25f);
        private Vector3 _FinalDestination;
        private float _SearchTime;
        private readonly float _MaxSearchTime = 1f;
        private float _Sight;
        private Transform _TargetTransform;


		private void Awake()
        {
            Assert.IsNotNull<NavMeshAgent>(_NavMeshAgent);
            Assert.IsNotNull<CapsuleCollider>(_Collider);
            enabled = false;
		}

        public void Init(NetworkNonPlayer.Side side, float speed, float sight)
        {
            enabled = true;

            _NavMeshAgent.enabled = true;
            _NavPath = new DynamicNavPath(_NavMeshAgent);
            
            currentState = AIState.Move;
            _Side = side;
            _Speed = speed;
            _Sight = sight;

            

            if(_Side == NetworkNonPlayer.Side.None)
                _FinalDestination = transform.position;

            if(_Side == NetworkNonPlayer.Side.Red)
                _FinalDestination = _FinalRedSideDestination;
            
            if(_Side == NetworkNonPlayer.Side.Blue)
                _FinalDestination = _FinalBlueSideDestination;

            SetMovementTarget(_FinalDestination);
            currentState = AIState.Move;
        }

        private void Start() {
            
        }

        void Update()
        {
            if(!IsServer)
                return;
                
			gameObject.SendMessage("SuperUpdate", SendMessageOptions.DontRequireReceiver);
        }

        protected override void EarlyGlobalSuperUpdate()
        {
	       
        }

        protected override void LateGlobalSuperUpdate()
        {
            if (!enabled) { return; }

		}

        #region States
        // Below are the state functions. Each one is called based on the name of the state, so when currentState = Idle,
        // we call Idle_EnterState. If currentState = Jump, we call Jump_SuperUpdate().

        private void Idle_EnterState()
        {
			Debug.Log("Idle_EnterState");
			_SearchTime = 0f;
        }

        // Run every frame character is in the idle state.
        private void Idle_SuperUpdate()
        {
			
        }

        private void Move_EnterState()
        {
			_SearchTime = 0f;
        }

        // Run every frame character is moving.
        private void Move_SuperUpdate()
        {
            _SearchTime -= Time.deltaTime;
            if(_SearchTime < 0f)
            {
                _SearchTime = _MaxSearchTime;
                Transform detectedTarget = DetectNonPlayerEnemy();
                if(detectedTarget != null)
                {
                    _TargetTransform = detectedTarget;
                    currentState = AIState.Chase;
                    return;
                }
            }

			var desiredMovementAmount = _Speed * Time.deltaTime;
            Vector3 movementVector = _NavPath.MoveAlongPath(desiredMovementAmount);
            if (movementVector == Vector3.zero)
            {
                currentState = AIState.Idle;
                return;
            }

            _NavMeshAgent.Move(movementVector);
            RotateTowardsDirection(movementVector);
            // After moving adjust the position of the dynamic rigidbody.
        }

        public Transform DetectNonPlayerEnemy()
        {
            Debug.LogWarning($"[test] DetectNonPlayerEnemy() : _Sight = {_Sight}");
            RaycastHit[] hitInfoArray = DetectNearbyEntities(true, false, _Collider, _Sight);
            Debug.LogWarning($"[test] DetectNonPlayerEnemy() hitInfoArray.Length = {hitInfoArray.Length}");
            foreach(var hitInfo in hitInfoArray)
            {
                var hitNetworkNonPlayer = hitInfo.collider.GetComponent<NetworkPlayer>();
                if(hitNetworkNonPlayer == null)
                    continue;
                
                //if(hitNetworkNonPlayer.CurrentSide == _Side)
                //    continue;

                return hitNetworkNonPlayer.transform;
            }
            Debug.LogWarning($"[test] DetectNonPlayerEnemy() Fail to detect!!!");
            return null;
        }

        public RaycastHit[] DetectNearbyEntities(bool wantPC, bool wantNPC, Collider attacker, float range)
        {
            int mask = 0;
            if (wantPC)
                mask |= (1 << LayerMask.NameToLayer("PC"));
            if (wantNPC)
                mask |= (1 << LayerMask.NameToLayer("NPC"));

            return Physics.SphereCastAll(attacker.transform.position, range, attacker.transform.up, 0f, mask);
        }

        private void Chase_EnterState()
        {
			Debug.Log("Chase Enter");
            FollowTransform(_TargetTransform);
            _SearchTime = 0f;

        }

        // Run every frame character is moving.
        private void Chase_SuperUpdate()
        {
			var desiredMovementAmount = _Speed * Time.deltaTime;
            Vector3 movementVector = _NavPath.MoveAlongPath(desiredMovementAmount);
            if (movementVector == Vector3.zero)
            {
                currentState = AIState.Idle;
                return;
            }

            _NavMeshAgent.Move(movementVector);
            RotateTowardsDirection(movementVector);
            // After moving adjust the position of the dynamic rigidbody.
        }

		#endregion

        private void RotateTowardsMovementDir()
        {
            
        }

        /*
        private void RotateTowardsTarget(Vector3 targetPosition)
        {
			if (debugMessages) { Debug.Log($"RotateTowardsTarget: {targetPosition}"); }
			var lookTarget = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);
			if (lookTarget != Vector3.zero) {
				var targetRotation = Quaternion.LookRotation(lookTarget);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
			}
        }
        */

		private void RotateTowardsDirection(Vector3 direction)
		{
			var lookDirection = new Vector3(direction.x, 0, -direction.y);
			var lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
		}

        public void SetMovementTarget(Vector3 position)
        {
            _NavPath.SetTargetPosition(position);
        }

        public void FollowTransform(Transform followTransform)
        {
            _NavPath.FollowTransform(followTransform);
        }

        public void CancelMove()
        {
            _NavPath?.Clear();
            currentState = AIState.Idle;
        }
    }
}
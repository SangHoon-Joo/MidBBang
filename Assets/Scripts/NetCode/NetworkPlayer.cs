using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;


namespace ProjectWilson
{
    public class NetworkPlayer : NetworkCharacter
    {
        [SerializeField]
		private RPGCharacterController _CharacterController;
		public RPGCharacterController CharacterController {	get { return _CharacterController; } }
		[SerializeField]
		private RPGCharacterMovementController _CharacterMovementController;
		public RPGCharacterMovementController CharacterMovementController {	get { return _CharacterMovementController; } }
        
        public override void OnNetworkSpawn()
        {
            Assert.IsNotNull<RPGCharacterController>(_CharacterController);
            Assert.IsNotNull<RPGCharacterMovementController>(_CharacterMovementController);

            base.OnNetworkSpawn();

            string locationTagName = IsHost ? "RedPlayerStartLocation" : "BluePlayerStartLocation";

            if (IsOwner)
            {
                CurrentSide = IsHost ? Side.Red : Side.Blue;

                if(SceneGamePlay.Instance == null || SceneGamePlay.Instance.UIGamePlayMain == null)
                    return;

                SceneGamePlay.Instance.UIGamePlayMain.SetMode(UIGamePlayMain.Mode.InPlay, this);

                InitPosition(locationTagName);
                
                _CharacterController.OnAttack += Attack;
                _CharacterMovementController.runSpeed = _TableData.Speed;

                NetworkObjectSpawnManager.Instance.SpawnAllNetworkObjects();
            }                
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsOwner)
            {
                _CharacterController.OnAttack -= Attack;
            }
        }

        private void InitPosition(string locationTagName)
        {
            var locationGameObject = GameObject.FindGameObjectWithTag(locationTagName);
            if(locationGameObject != null)
            {
                transform.SetPositionAndRotation(locationGameObject.transform.position, locationGameObject.transform.rotation);
            }
        }

        protected override void Attack(float range, int amount)
        {
            base.Attack(range, amount);
        }
    }
}
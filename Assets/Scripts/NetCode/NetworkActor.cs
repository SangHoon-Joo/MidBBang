using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectWilson.Gameplay.GameplayObjects;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;

namespace ProjectWilson
{
    [RequireComponent(typeof(NetworkHealthState), typeof(NetworkLifeState), typeof(NetworkNameState))]
    public class NetworkActor : NetworkBehaviour
    {
        [SerializeField]
		protected NetworkNameState _CharacterNameState;
		public NetworkNameState CharacterNameState { get { return _CharacterNameState; } }
        [SerializeField]
		protected NetworkHealthState _CharacterHPState;
		public NetworkHealthState CharacterHPState { get { return _CharacterHPState; } }
        [SerializeField]
		protected NetworkLifeState _CharacterLifeState;
		public NetworkLifeState CharacterLifeState { get { return _CharacterLifeState; } }
        [SerializeField]
        protected Collider _Collider;
        public Collider Collider { get { return _Collider; } }
        [SerializeField]
		protected int _LocalID = 0;
		public int LocalID { protected set { _LocalID = value; } get { return _LocalID; } }
        public FixedPlayerName Name
        {
            get => _CharacterNameState.Name.Value;
            protected set => _CharacterNameState.Name.Value = value;
        }
        public int HP
        {
            get => _CharacterHPState.HP.Value;
            protected set => _CharacterHPState.HP.Value = value;
        }
        public int MaxHP
        {
            get => _CharacterHPState.MaxHP.Value;
            protected set => _CharacterHPState.MaxHP.Value = value;
        }
        public LifeState LifeState
        {
            get => _CharacterLifeState.Life.Value;
            protected set => _CharacterLifeState.Life.Value = value;
        }
		public bool IsAlive	=> _CharacterLifeState.Life.Value == LifeState.Alive;
        public bool IsPC { protected set; get; }
        protected ActorData _TableData;
		public ActorData TableData { get { return _TableData; } }
        
        public override async void OnNetworkSpawn()
        {
            Assert.IsNotNull<NetworkNameState>(_CharacterNameState);
            Assert.IsNotNull<NetworkHealthState>(_CharacterHPState);
			Assert.IsNotNull<NetworkLifeState>(_CharacterLifeState);
            Assert.IsNotNull<Collider>(_Collider);

            _CharacterHPState.HP.OnValueChanged += OnHPStateChanged;
            _CharacterHPState.MaxHP.OnValueChanged += OnMaxHPStateChanged;
            _CharacterLifeState.Life.OnValueChanged += OnLifeStateChanged;
            _TableData = TableDataManager.Instance.Actor.Data[_LocalID];
            IsPC = _TableData.Kind == ActorData.Kinds.PC;

            if (IsOwner)
            {
                if(IsPC)
                    Name = GameDataManager.Instance.Player.Name;
                else
                    Name = _TableData.Name;
                
                _CharacterHPState.HPDepleted += OnDepleteHP;
                
                if (IsServer)
                {
                    HP = _TableData.HP;
                    MaxHP = HP;
                }
                else
                    SubmitInitHPRequestServerRpc(_TableData.HP);

                CameraManager.Instance.ChangeMode(CameraManager.Mode.Default, this);
            }

            int retryCount = 0;
			for(;;)
			{
				if(!string.IsNullOrEmpty(Name))
					break;

				retryCount++;
				if(retryCount >= 50)
				{
					Debug.LogError("Name is string.Empty!!!");
					return;
				}
				await Task.Delay(100);
			}
            await LoadUIs();
            
            _UIGamePlayWorldActorState.Show();
        }

        [ServerRpc]
        void SubmitInitHPRequestServerRpc(int hp)
        {
            HP = hp;
            MaxHP = HP;
        }

        public override void OnNetworkDespawn()
        {
            Debug.LogWarning($"[test] OnNetworkDespawn()");
            _CharacterHPState.HP.OnValueChanged -= OnHPStateChanged;
            _CharacterHPState.MaxHP.OnValueChanged -= OnMaxHPStateChanged;
            _CharacterLifeState.Life.OnValueChanged -= OnLifeStateChanged;
            if (IsOwner)
            {
                _CharacterHPState.HPDepleted -= OnDepleteHP;
            }
            UnloadUIs();
        }

        protected virtual void OnHPStateChanged(int prevHPState, int newHPState)
        {
            _UIGamePlayWorldActorState?.RefreshHPFillAmount();
        }

        protected virtual void OnMaxHPStateChanged(int prevMaxHPState, int newMaxHPState)
        {
            _UIGamePlayWorldActorState?.RefreshHPFillAmount();
        }

        protected virtual void OnLifeStateChanged(LifeState prevLifeState, LifeState newLifeState)
        {
            if(prevLifeState == LifeState.Alive && newLifeState == LifeState.Dead)
            {
                if(IsPC)
                {
                    if(IsOwner)
                    {
                        SceneGamePlay.Instance.UIGamePlayMain.GameOver(false);
                    }
                    else
                    {
                        SceneGamePlay.Instance.UIGamePlayMain.GameOver(true);
                    }
                }
                else
                {
                    NetworkObject.Despawn(true);
                }
            }
        }

        public void ReceiveHP(int hp)
        {
            if(IsServer)
                HP = (int)Mathf.Clamp(HP + hp, 0f, MaxHP);
            else
                SubmitReceiveHPRequestServerRpc(hp);
        }

        [ServerRpc(RequireOwnership = false)]
        void SubmitReceiveHPRequestServerRpc(int hp)
        {
            HP = (int)Mathf.Clamp(HP + hp, 0f, MaxHP);
        }

        protected virtual void OnDepleteHP()
        {
            if(IsServer)
                LifeState = LifeState.Dead;
            else
                SubmitOnDepleteHPRequestServerRpc();
        }

        [ServerRpc]
        void SubmitOnDepleteHPRequestServerRpc()
        {
            LifeState = LifeState.Dead;
        }

        protected UIGamePlayWorldActorState _UIGamePlayWorldActorState;
        private async Task LoadUIs()
		{
			string uiGamePlayWorldActorStatePath = "Assets/AddressableResources/Prefabs/UIs/UIGamePlayWorldActorState.prefab";
			AsyncOperationHandle<GameObject> uiGamePlayWorldActorStateHandle = Addressables.InstantiateAsync(uiGamePlayWorldActorStatePath);

			await uiGamePlayWorldActorStateHandle.Task;

			if(uiGamePlayWorldActorStateHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject uiGamePlayWorldActorStateGameObject = uiGamePlayWorldActorStateHandle.Result;
				uiGamePlayWorldActorStateGameObject.name = uiGamePlayWorldActorStateHandle.Result.name.Replace("(Clone)", string.Empty);
				_UIGamePlayWorldActorState = uiGamePlayWorldActorStateGameObject.GetComponent<UIGamePlayWorldActorState>();
				_UIGamePlayWorldActorState.Attach(this, uiGamePlayWorldActorStateHandle.Result.transform as RectTransform);
			}
		}

		private void UnloadUIs()
		{
			if(_UIGamePlayWorldActorState != null)
			{
				_UIGamePlayWorldActorState.Hide();
				_UIGamePlayWorldActorState.Detach();
				Addressables.ReleaseInstance(_UIGamePlayWorldActorState.gameObject);
				_UIGamePlayWorldActorState = null;
			}
		}
    }
}
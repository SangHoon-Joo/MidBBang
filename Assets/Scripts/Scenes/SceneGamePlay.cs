using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;
using Unity​Engine.Assertions;
using Krafton.SP2.X1.Lib;
//using Lean.Touch;

namespace ProjectWilson
{
	public class SceneGamePlay : MonoBehaviourTemporarySingleton<SceneGamePlay>
	{		
		public bool IsInitialized { private set; get; }

		//[SerializeField]
		//private World _World;
		//public World World { get { return _World; } }
		//[SerializeField]
		//private Actor _PC;
		//public Actor PC { get { return _PC; } }

		public UIGamePlayMain UIGamePlayMain { private set; get; }

		protected override async void Awake()
		{
			base.Awake();

			//Assert.IsNotNull<World>(_World);
			//Assert.IsNotNull<Actor>(_PC);

			await Init();
		}

		protected override void OnDestroy()
		{
			IsInitialized = false;

			Unload();

			base.OnDestroy();
		}

		private void OnEnable()
		{
			//LeanTouch.OnFingerDown += OnFingerDown;
			//LeanTouch.OnFingerUp += OnFingerUp;
			//LeanTouch.OnFingerTap += OnFingerTap;
		}

		private void OnDisable()
		{
			//LeanTouch.OnFingerDown -= OnFingerDown;
			//LeanTouch.OnFingerUp -= OnFingerUp;
			//LeanTouch.OnFingerTap -= OnFingerTap;
		}

		private async Task Init()
		{
			//Physics.gravity = Vector3.zero;
			await Load();
			IsInitialized = true;
		}

		private async Task Load()
		{
			string uiGameMainPath = "Assets/AddressableResources/Prefabs/UIs/UIGamePlayMain.prefab";

			AsyncOperationHandle<GameObject> uiGameMainHandle = Addressables.InstantiateAsync(uiGameMainPath);

			await uiGameMainHandle.Task;

			if(uiGameMainHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject uiGameMainGameObject = uiGameMainHandle.Result;
				uiGameMainGameObject.name = uiGameMainHandle.Result.name.Replace("(Clone)", string.Empty);
				UIHorizontalManager.Instance.AttachUI(uiGameMainGameObject, uiGameMainHandle.Result.transform as RectTransform, UIHorizontalManager.CanvasType.Main);
				UIGamePlayMain = uiGameMainGameObject.GetComponent<UIGamePlayMain>();
				UIGamePlayMain.SetMode(UIGamePlayMain.Mode.Default);
			}
		}

		private async void Start()
		{
			int retryCount = 0;
			for(;;)
			{
				if(UIGamePlayMain != null)
					break;

				retryCount++;
				if(retryCount >= 50)
				{
					Debug.LogError("UIGamePlayMain is null!!!");
					return;
				}
				await Task.Delay(100);
			}

			if(RelayManager.Instance.CreatedRelay)
			{
				RelayManager.Instance.SetHostRelayData();
				NetworkManager.Singleton.StartHost();
			}
			else
			{
				RelayManager.Instance.SetClientRelayData();
				NetworkManager.Singleton.StartClient();
			}
				
		}

		private void Unload()
		{
			if(UIGamePlayMain != null)
			{
				Addressables.ReleaseInstance(UIGamePlayMain.gameObject);
				UIGamePlayMain = null;
			}
		}

		private void Update()
		{
			if(!IsInitialized)
				return;

			if(UIGamePlayMain == null)
				return;
		}

		/*
		private bool _IsValidFingerTap = false;
		private void OnFingerDown(LeanFinger leanFinger)
		{
			if(!IsInitialized)
				return;

			if(LeanTouch.Fingers.Count > 1)
				return;

			if(UIManager.Instance.IsPositionOverGameObject(leanFinger.ScreenPosition))
				return;

			_IsValidFingerTap = true;

			if(UIGameMain == null || UIGameMain.GetMode() != UIGameMain.Mode.Default)
				return;

			if(CameraManager.Instance == null || CameraManager.Instance.IsInitialized == false || CameraManager.Instance.IsInTransition())
				return;

			if(CameraManager.Instance.IsLocked)
				return;

			Ray ray = CameraManager.Instance.GetCamera().ScreenPointToRay(leanFinger.ScreenPosition);
			RaycastHit raycastHit;
			if(!Physics.Raycast(ray, out raycastHit))
				return;
				
			Actor actor = raycastHit.transform.GetComponent<Actor>();
			if(actor != null && actor is Prop prop)
			{
				if(TableDataManager.Instance.Prop.Data[actor.ID].HelpID != 0 && !GameDataManager.Instance.Player.HelpManager.IsExposed(TableDataManager.Instance.Prop.Data[actor.ID].HelpID))
				{
					if(UIGameMain.UIGameMainHelpPage.Init(UIGameMainHelpPage.Kinds.Single, TableDataManager.Instance.Prop.Data[actor.ID].HelpID))
					{
						UIGameMain.UIGameMainHelpPage.gameObject.SetActive(true);
						return;
					}
				}

				_SelectedProp = prop;
				_SelectedProp.StartHarvest();
			}
		}

		private void OnFingerUp(LeanFinger leanFinger)
		{
			_IsValidFingerTap = false;

			if(!IsInitialized)
				return;

			if(LeanTouch.Fingers.Count > 1)
				return;

			if(UIManager.Instance.IsPositionOverGameObject(leanFinger.ScreenPosition))
				return;

			if(UIGameMain == null || UIGameMain.GetMode() != UIGameMain.Mode.Default)
				return;

			if(CameraManager.Instance == null || CameraManager.Instance.IsInitialized == false || CameraManager.Instance.IsInTransition())
				return;

			if(CameraManager.Instance.IsLocked)
				return;

			if(_SelectedProp != null)
			{
				_SelectedProp.CancelHarvest();
				_SelectedProp = null;
			}
		}

		private void OnFingerTap(LeanFinger leanFinger)
		{
			if(!IsInitialized)
				return;

			if(LeanTouch.Fingers.Count > 1)
				return;

			if(UIManager.Instance.IsPositionOverGameObject(leanFinger.ScreenPosition))
				return;

			bool isValidFingerTap = _IsValidFingerTap;
			_IsValidFingerTap = false;

			if(!isValidFingerTap)
				return;

			if(UIGameMain == null || UIGameMain.GetMode() != UIGameMain.Mode.Default)
				return;
		
			if(CameraManager.Instance == null || CameraManager.Instance.IsInitialized == false || CameraManager.Instance.IsInTransition())
				return;

			Ray ray;
			if(CameraManager.Instance.IsOn())
				ray = CameraManager.Instance.GetCamera().ScreenPointToRay(leanFinger.ScreenPosition);
			else
				ray = CutSceneCameraManager.Instance.GetCamera().ScreenPointToRay(leanFinger.ScreenPosition);
			RaycastHit raycastHit;
			if(!Physics.Raycast(ray, out raycastHit))
				return;

			if(raycastHit.transform.tag == "ClickTarget")
			{
				Actor actor = raycastHit.transform.GetComponentInParent<Actor>();
				if(actor != null)
				{
					actor.IsActionClickTargetClicked = true;
					SoundManager.Instance.PlaySFX(SoundManager.SFX.SFX_Button);
				}
				return;
			}

			if(CameraManager.Instance.IsLocked)
				return;

			Character character = raycastHit.transform.GetComponent<Character>();
			if(character != null)
			{
				ClickCharacter(character);
				return;
			}

			Prop prop = raycastHit.transform.GetComponent<Prop>();
			if(prop != null)
			{
				ClickProp(prop);
				return;
			}

			SectorCloud sectorCloud = raycastHit.transform.GetComponent<SectorCloud>();
			if(sectorCloud != null)
			{
				ClickSectorCloud(sectorCloud);
				return;
			}

			SectorBoard sectorBoard = raycastHit.transform.GetComponent<SectorBoard>();
			if(sectorBoard != null)
			{
				ClickSectorBoard(sectorBoard);
				return;
			}
		}
		
		public void ClickSectorCloud(SectorCloud sectorCloud)
		{
			UIGameMain.SetMode(UIGameMain.Mode.SectorInteraction, sectorCloud);
			//SoundManager.Instance.PlaySFX(SoundManager.SFX.SFX_Upgrade);
		}
		*/
	}
}
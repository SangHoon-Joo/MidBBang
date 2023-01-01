using System;
using UnityEngine;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class CameraManager : MonoBehaviourTemporarySingleton<CameraManager>
	{
		[SerializeField]
		private Transform _RootTransform;
		[SerializeField]
		private Transform _RotateArmTransform;
		[SerializeField]
		private Transform _ShiftArmTransform;
		[SerializeField]
		private Transform _DistanceArmTransform;
		[SerializeField]
		private Camera _Camera;
		[SerializeField]
		private Transform _CameraTransform;
		public enum Mode
		{
			Default,
			GameOver,
			Max
		}
		private Mode _Mode;
		[Serializable]
		private struct CameraInfo
		{
			public float FOV;
			public float Pitch;
			public float Distance;
		}
		[SerializeField]
		private float _DistanceMin;
		[SerializeField]
		private float _DistanceMax;
		[SerializeField]
		private CameraInfo _DefaultCameraInfo;
		private CameraInfo _CurrentCameraInfo;
		private Vector3 _CurrentShift;
		private float _CurrentYaw;
		private CameraInfo _TargetCameraInfo;
		private Vector3 _TargetShift;
		private float _TargetYaw;
		private Vector3 _TargetPosition;
		private NetworkActor _TargetActor;
		private bool _IsInTransition;
		private bool _IsTargetFocusFinished;
		private const float _LerpValue = 0.2f;
		public bool IsLocked { private set; get; }
		private int _LockCount;
		public bool IsInitialized {	private set; get; }

		protected override void Awake()
		{
			base.Awake();

			Initialize();
		}

		private void Initialize()
		{
			Assert.IsNotNull<Transform>(_RootTransform);
			Assert.IsNotNull<Transform>(_DistanceArmTransform);
			Assert.IsNotNull<Transform>(_ShiftArmTransform);
			Assert.IsNotNull<Camera>(_Camera);
			Assert.IsNotNull<Transform>(_CameraTransform);

			_Mode = Mode.Default;

			//_RootTransform.position = GameDataManager.Instance.Camera.Position;
			//if(_RootTransform.position.x < TableDataManager.Instance.Setting.Data.CameraXLimitMin || _RootTransform.position.x > TableDataManager.Instance.Setting.Data.CameraXLimitMax || _RootTransform.position.z < TableDataManager.Instance.Setting.Data.CameraZLimitMin || _RootTransform.position.z > TableDataManager.Instance.Setting.Data.CameraZLimitMax)
			_RootTransform.position = Vector3.zero;

			//if(GameDataManager.Instance.Camera.Distance == 0.0f)
			//	GameDataManager.Instance.Camera.Distance = _DefaultCameraInfo.Distance;
			_CurrentCameraInfo = _DefaultCameraInfo;
			//_CurrentCameraInfo.Distance = GameDataManager.Instance.Camera.Distance;
			_CurrentShift = Vector3.zero;
			_CurrentYaw = 0;
			_TargetCameraInfo = _DefaultCameraInfo;
			//_TargetCameraInfo.Distance = GameDataManager.Instance.Camera.Distance;
			_TargetShift = Vector3.zero;
			_TargetYaw = 0;

			_ShiftArmTransform.localPosition = _CurrentShift;
			_DistanceArmTransform.localPosition = new Vector3(0.0f, _CurrentCameraInfo.Distance * Mathf.Sin(_CurrentCameraInfo.Pitch * Mathf.Deg2Rad), -_CurrentCameraInfo.Distance * Mathf.Cos(_CurrentCameraInfo.Pitch * Mathf.Deg2Rad));
			_CameraTransform.localEulerAngles = new Vector3(_CurrentCameraInfo.Pitch, 0.0f, 0.0f);
			_Camera.fieldOfView = _CurrentCameraInfo.FOV;

			_TargetPosition = _RootTransform.position;
			_TargetActor = null;

			Unlock();

			IsInitialized = true;
			On();
		}

		public Camera GetCamera()
		{
			return _Camera;
		}

		public Vector3 GetForward()
		{
			return _CameraTransform.forward;
		}

		public void On()
		{
			gameObject.SetActive(true);
		}

		public void Off()
		{
			gameObject.SetActive(false);
		}

		public bool IsOn()
		{
			return gameObject.activeSelf;
		}

		public void Lock()
		{
			IsLocked = true;
			_LockCount++;
		}

		public void Unlock()
		{
			_LockCount--;
			if(_LockCount > 0)
				return;

			IsLocked = false;
			_LockCount = 0;
		}

		public int GetLockCount()
		{
			return _LockCount;
		}

		public float GetFOV()
		{
			return _CurrentCameraInfo.FOV;
		}

		public float GetPitch()
		{
			return _CurrentCameraInfo.Pitch;
		}

		public float GetYaw()
		{
			return _CurrentYaw;
		}

		public float GetDistance()
		{
			return _CurrentCameraInfo.Distance;
		}

		public Vector3 GetShift()
		{
			return _CurrentShift;
		}

		public Vector3 GetRootPosition()
		{
			return _RootTransform.position;
		}

		public Vector3 GetRootForward()
		{
			return _RootTransform.forward;
		}

		public Vector3 GetPosition()
		{
			return _CameraTransform.position;
		}

		public float GetUIScale()
		{
			return Mathf.Max(1.35f - 0.025f * _CurrentCameraInfo.Distance, 0.5f);
		}

		public void ChangeMode(Mode mode, NetworkActor targetActor = null)
		{
			Mode previousMode = _Mode;
			_Mode = mode;

			NetworkActor previousTargetActor = _TargetActor;
			if(_TargetActor == null)
				_TargetActor = targetActor;
			
			if(_Mode != previousMode || _TargetActor != previousTargetActor)
			{
				_TargetShift = Vector3.zero;
				_TargetYaw = 0;

				if(_Mode == Mode.GameOver)
				{
					_TargetCameraInfo = _DefaultCameraInfo;
				}
				else
					_TargetCameraInfo = _DefaultCameraInfo;

				_TargetPosition = _RootTransform.position;

				_IsInTransition = true;
				_IsTargetFocusFinished = false;
			}
		}

		public Mode GetMode()
		{
			return _Mode;
		}

		public void ResetTargetActor()
		{
			_TargetActor = null;
			_TargetPosition = _RootTransform.position;
		}

		public bool IsInTransition()
		{
			return _IsInTransition;
		}

		public void MoveDelta(float forwardDelta, float rightDelta, bool ignoreLock = false)
		{
			if((!ignoreLock && IsLocked) || _Mode != Mode.Default || _IsInTransition)
				return;

			_TargetPosition += _RootTransform.forward * forwardDelta + _RootTransform.right * rightDelta;
			//_TargetPosition = ScenePlay.Instance.World.GetTerrainPosition(_TargetPosition, World.TerrainKind.Ground);
		}

		public void MoveDelta(Vector2 delta, bool ignoreLock = false)
		{
			if((!ignoreLock && IsLocked) || _Mode != Mode.Default || _IsInTransition)
				return;

			Vector3 worldDelta = _Camera.ScreenToWorldPoint(new Vector3(delta.x, delta.y, _CurrentCameraInfo.Distance)) - _Camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, _CurrentCameraInfo.Distance));
			float forwardDelta = Vector3.Dot(worldDelta, Vector3.Normalize(_RootTransform.up + _RootTransform.forward)) / Mathf.Cos((90 - _CurrentCameraInfo.Pitch) * Mathf.Deg2Rad);
			float rightDelta = Vector3.Dot(worldDelta, _RootTransform.right);
			_TargetPosition += _RootTransform.forward * forwardDelta + _RootTransform.right * rightDelta;
			//_TargetPosition = ScenePlay.Instance.World.GetTerrainPosition(_TargetPosition, World.TerrainKind.Ground);
		}

		public bool Move(Vector3 targetPosition, bool ignoreLock = false)
		{
			if((!ignoreLock && IsLocked) || _Mode != Mode.Default || _IsInTransition)
				return false;

			_TargetPosition = targetPosition;
			//_TargetPosition = ScenePlay.Instance.World.GetTerrainPosition(_TargetPosition, World.TerrainKind.Ground);
			return true;
		}

		public void Zoom(float change, bool ignoreLock = false)
		{
			if((!ignoreLock && IsLocked) || _Mode != Mode.Default || _IsInTransition)
				return;

			_TargetCameraInfo.Distance = Mathf.Clamp(_TargetCameraInfo.Distance + change, _DistanceMin, _DistanceMax);
		}

		public void Update()
		{
			bool isChangeFinished = true;

			// FOV
			if(_CurrentCameraInfo.FOV != _TargetCameraInfo.FOV)
			{	
				_CurrentCameraInfo.FOV = Mathf.Lerp(_CurrentCameraInfo.FOV, _TargetCameraInfo.FOV, _LerpValue);
					
				if(Mathf.RoundToInt(_TargetCameraInfo.FOV * 100.0f) == Mathf.RoundToInt(_CurrentCameraInfo.FOV * 100.0f) || Mathf.Abs(_TargetCameraInfo.FOV - _CurrentCameraInfo.FOV) <= Mathf.Epsilon)
					_CurrentCameraInfo.FOV = _TargetCameraInfo.FOV;
				else
					isChangeFinished = false;

				_Camera.fieldOfView = _CurrentCameraInfo.FOV;
			}

			// Yaw
			if(_CurrentYaw != _TargetYaw)
			{	
				_CurrentYaw = Mathf.Lerp(_CurrentYaw, _TargetYaw, _LerpValue);
					
				if(Mathf.RoundToInt(_TargetYaw * 100.0f) == Mathf.RoundToInt(_CurrentYaw * 100.0f) || Mathf.Abs(_TargetYaw - _CurrentYaw) <= Mathf.Epsilon)
					_CurrentYaw = _TargetYaw;
				else
					isChangeFinished = false;

				_RotateArmTransform.localEulerAngles = new Vector3(0.0f, _CurrentYaw, 0.0f);
			}

			bool updatePosition = false;

			// Pitch
			if(_CurrentCameraInfo.Pitch != _TargetCameraInfo.Pitch)
			{	
				_CurrentCameraInfo.Pitch = Mathf.Lerp(_CurrentCameraInfo.Pitch, _TargetCameraInfo.Pitch, _LerpValue);
					
				if(Mathf.RoundToInt(_TargetCameraInfo.Pitch * 100.0f) == Mathf.RoundToInt(_CurrentCameraInfo.Pitch * 100.0f) || Mathf.Abs(_TargetCameraInfo.Pitch - _CurrentCameraInfo.Pitch) <= Mathf.Epsilon)
					_CurrentCameraInfo.Pitch = _TargetCameraInfo.Pitch;
				else
					isChangeFinished = false;

				_CameraTransform.localEulerAngles = new Vector3(_CurrentCameraInfo.Pitch, 0.0f, 0.0f);

				updatePosition = true;
			}

			// Distance
			if(_CurrentCameraInfo.Distance != _TargetCameraInfo.Distance)
			{	
				_CurrentCameraInfo.Distance = Mathf.Lerp(_CurrentCameraInfo.Distance, _TargetCameraInfo.Distance, _LerpValue);
					
				if(Mathf.RoundToInt(_TargetCameraInfo.Distance * 100.0f) == Mathf.RoundToInt(_CurrentCameraInfo.Distance * 100.0f) || Mathf.Abs(_TargetCameraInfo.Distance - _CurrentCameraInfo.Distance) <= Mathf.Epsilon)
					_CurrentCameraInfo.Distance = _TargetCameraInfo.Distance;
				else
					isChangeFinished = false;
				
				//if(_Mode == Mode.Default && !_IsInTransition)
				//	GameDataManager.Instance.Camera.Distance = Mathf.Clamp(_CurrentCameraInfo.Distance, _DistanceMin, _DistanceMax);

				updatePosition = true;
			}

			if(updatePosition)
				_DistanceArmTransform.localPosition = new Vector3(0.0f, _CurrentCameraInfo.Distance * Mathf.Sin(_CurrentCameraInfo.Pitch * Mathf.Deg2Rad), -_CurrentCameraInfo.Distance * Mathf.Cos(_CurrentCameraInfo.Pitch * Mathf.Deg2Rad));

			// Shift
			if(_CurrentShift != _TargetShift)
			{	
				_CurrentShift = Vector3.Lerp(_CurrentShift, _TargetShift, _LerpValue);

				if((Mathf.RoundToInt(_TargetShift.x * 100.0f) == Mathf.RoundToInt(_CurrentShift.x * 100.0f) && Mathf.RoundToInt(_TargetShift.y * 100.0f) == Mathf.RoundToInt(_CurrentShift.y * 100.0f) && Mathf.RoundToInt(_TargetShift.z * 100.0f) == Mathf.RoundToInt(_CurrentShift.z * 100.0f)) || (Mathf.Abs(_TargetShift.x - _CurrentShift.x) <= Mathf.Epsilon && Mathf.Abs(_TargetShift.y - _CurrentShift.y) <= Mathf.Epsilon && Mathf.Abs(_TargetShift.z - _CurrentShift.z) <= Mathf.Epsilon))
					_CurrentShift = _TargetShift;
				else
					isChangeFinished = false;

				_ShiftArmTransform.localPosition = _CurrentShift;
			}
			
			if(_TargetActor != null)
			{
				if(_IsTargetFocusFinished)
					_RootTransform.position = _TargetActor.transform.position;
				else
				{
					Vector3 previousRootTransformPosition = _RootTransform.position;
					_RootTransform.position = Vector3.Lerp(_RootTransform.position, _TargetActor.transform.position, _LerpValue);
					if(_RootTransform.position == previousRootTransformPosition)
						_IsTargetFocusFinished = true;
					else
						isChangeFinished = false;
				}
				//GameDataManager.Instance.Camera.Position = _RootTransform.position;
			}
			else if(_TargetPosition != _RootTransform.position)
			{
				_RootTransform.position = Vector3.Lerp(_RootTransform.position, _TargetPosition, _LerpValue);

				if(Vector3.SqrMagnitude(_TargetPosition - _RootTransform.position) <= 0.01f)
					_RootTransform.position = _TargetPosition;
				else
					isChangeFinished = false;

				//GameDataManager.Instance.Camera.Position = _RootTransform.position;
			}

			// Transition Finish
			if(_IsInTransition && isChangeFinished == true)
				_IsInTransition = false;
		}
	}
}
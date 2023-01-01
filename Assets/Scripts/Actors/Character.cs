using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;
using UnityEngine.Assertions;

namespace ProjectWilson
{
	public class Character : Actor
	{
		/*
		private class PlayAnimationFinishCallback
		{
			public System.Action Callback;

			public PlayAnimationFinishCallback(System.Action callback)
			{
				Callback = callback;
			}
		}

		private PlayAnimationFinishCallback _PlayAnimationFinishCallback;

		public void PlayAnimation(CharacterAnimationData.MotionKinds motionAnimationKind = CharacterAnimationData.MotionKinds.Skip, CharacterAnimationData.FaceKinds faceAnimationKind = CharacterAnimationData.FaceKinds.Skip, System.Action finishCallback = null, float finishTime = 0.0f)
		{
			if(_Animator == null)
			{
				ExecutePlayAnimationCallback(finishCallback, finishTime, false);
				return;
			}			
			
			bool needToCheckAnimationFinish = false;
			if(motionAnimationKind != CharacterAnimationData.MotionKinds.Skip)
			{
				_Animator.SetInteger(CharacterAnimationData.AnimatorMotionCtrlParamName, (int)motionAnimationKind);
				needToCheckAnimationFinish = true;
			}
			
			if(faceAnimationKind != CharacterAnimationData.FaceKinds.Skip)
			{
				if(faceAnimationKind == CharacterAnimationData.FaceKinds.Idle)
					_Animator.SetLayerWeight((int)CharacterAnimationData.AnimatorLayerKinds.FaceLayer, 0.0f);
				else
					_Animator.SetLayerWeight((int)CharacterAnimationData.AnimatorLayerKinds.FaceLayer, 1.0f);

				_Animator.SetInteger(CharacterAnimationData.AnimatorFaceCtrlParamName, (int)faceAnimationKind);
			}
			
			ExecutePlayAnimationCallback(finishCallback, finishTime, needToCheckAnimationFinish);
		}

		private void ExecutePlayAnimationCallback(System.Action finishCallback, float finishTime, bool needToCheckAnimationFinish)
		{
			if(_AnimCoroutine != null)
			{
				StopCoroutine(_AnimCoroutine);
				_AnimCoroutine = null;
			}

			PlayAnimationFinishCallback previousPlayAnimationFinishCallback = _PlayAnimationFinishCallback;
			if(finishCallback == null)
			{
				_PlayAnimationFinishCallback = null;
				previousPlayAnimationFinishCallback?.Callback?.Invoke();
				return;
			}

			_PlayAnimationFinishCallback = new PlayAnimationFinishCallback(finishCallback);
			PlayAnimationFinishCallback currentPlayAnimationFinishCallback = _PlayAnimationFinishCallback;
			previousPlayAnimationFinishCallback?.Callback?.Invoke();
			if(currentPlayAnimationFinishCallback == _PlayAnimationFinishCallback)
				_AnimCoroutine = StartCoroutine(AnimationCallbackProcess(finishTime, needToCheckAnimationFinish));
		}
		*/

		/*
		private UIHUDDialogue _UIHUDDialogue = null;
		public void ShowDialogue(string dialogue, float displayDuration = 3.0f)
		{
			if(string.IsNullOrEmpty(dialogue))
				return;

			if(_UIHUDDialogue != null)
				_UIHUDDialogue.Show(dialogue, displayDuration);
		}

		public void HideDialogue()
		{
			if(_UIHUDDialogue != null)
				_UIHUDDialogue.Hide();
		}

		public void SetDialogueAlpha(float alpha)
		{
			if(_UIHUDDialogue != null)
				_UIHUDDialogue.SetAlpha(alpha);
		}
		*/
		
		[SerializeField]
		private RPGCharacterController _CharacterController;
		public RPGCharacterController CharacterController {	get { return _CharacterController; } }
		[SerializeField]
		private RPGCharacterMovementController _CharacterMovementController;
		public RPGCharacterMovementController CharacterMovementController {	get { return _CharacterMovementController; } }
		protected ActorData _TableData;

		public ActorData TableData { get { return _TableData; } }

		protected override void Awake()
		{
			base.Awake();

			_CharacterController = GetComponent<RPGCharacterController>();
			_CharacterMovementController = GetComponent<RPGCharacterMovementController>();

			Assert.IsNotNull<RPGCharacterController>(_CharacterController);
			Assert.IsNotNull<RPGCharacterMovementController>(_CharacterMovementController);
			
			Init();
		}

		private void Init()
		{
			_IsInitialized = true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override /*async*/ void Start()
		{
			if(!_IsInitialized)
				return;

			base.Start();
			
			
			_TableData = TableDataManager.Instance.Actor.Data[ID];

			Name = _TableData.Name;
			Sight = _TableData.Sight;

			InitMove(_TableData.Speed);

			//await LoadUIs();
		}

		/*
		private async Task LoadUIs()
		{
			string uiHUDDialoguePath = "Assets/AddressableResources/Prefabs/UIs/HUD/UIHUDDialogue.prefab";
			AsyncOperationHandle<GameObject> uiHUDDialogueHandle = Addressables.InstantiateAsync(uiHUDDialoguePath);

			await uiHUDDialogueHandle.Task;

			if(uiHUDDialogueHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject uiHUDDialogueGameObject = uiHUDDialogueHandle.Result;
				uiHUDDialogueGameObject.name = uiHUDDialogueHandle.Result.name.Replace("(Clone)", string.Empty);
				_UIHUDDialogue = uiHUDDialogueGameObject.GetComponent<UIHUDDialogue>();
				_UIHUDDialogue.Attach(this, uiHUDDialogueHandle.Result.transform as RectTransform);
			}
		}

		private void UnloadUIs()
		{
			if(_UIHUDDialogue != null)
			{
				_UIHUDDialogue.Hide();
				_UIHUDDialogue.Detach();
				Addressables.ReleaseInstance(_UIHUDDialogue.gameObject);
				_UIHUDDialogue = null;
			}
		}
		*/

		private void InitMove(float speed)
		{
			if(_CharacterMovementController != null)
				_CharacterMovementController.runSpeed = speed;
		}
		
		protected override void Update()
		{
			if(!_IsInitialized)
				return;
				
			base.Update();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			//UnloadUIs();
		}
	}
}
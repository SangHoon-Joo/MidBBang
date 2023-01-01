using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;
using Unity​Engine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class SceneGameLobby : MonoBehaviourTemporarySingleton<SceneGameLobby>
	{
		public UIGameLobbyMain UIGameLobbyMain { private set; get; }

		protected override async void Awake()
		{
			base.Awake();
			await Load();
		}

		protected override void OnDestroy()
		{
			Unload();
			base.OnDestroy();
		}

		private async Task Load()
		{
			string uiGameLobbyMainPath = "Assets/AddressableResources/Prefabs/UIs/UIGameLobbyMain.prefab";

			AsyncOperationHandle<GameObject> uiGameLobbyMainHandle = Addressables.InstantiateAsync(uiGameLobbyMainPath);

			await uiGameLobbyMainHandle.Task;

			if(uiGameLobbyMainHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject uiGameLobbyMainGameObject = uiGameLobbyMainHandle.Result;
				uiGameLobbyMainGameObject.name = uiGameLobbyMainHandle.Result.name.Replace("(Clone)", string.Empty);
				UIHorizontalManager.Instance.AttachUI(uiGameLobbyMainGameObject, uiGameLobbyMainHandle.Result.transform as RectTransform, UIHorizontalManager.CanvasType.Main);
				UIGameLobbyMain = uiGameLobbyMainGameObject.GetComponent<UIGameLobbyMain>();
				UIGameLobbyMain.SetMode(UIGameLobbyMain.Mode.SearchLobby);
			}
		}

		private void Unload()
		{
			if(UIGameLobbyMain != null)
			{
				Addressables.ReleaseInstance(UIGameLobbyMain.gameObject);
				UIGameLobbyMain = null;
			}
		}
	}
}
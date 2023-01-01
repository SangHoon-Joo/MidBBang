using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class SceneStart : MonoBehaviourTemporarySingleton<SceneStart>
	{
		public UIStartMain UIStartMain { private set; get; }

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
			string uiStartMainPath = "Assets/AddressableResources/Prefabs/UIs/UIStartMain.prefab";

			AsyncOperationHandle<GameObject> uiStartMainHandle = Addressables.InstantiateAsync(uiStartMainPath);

			await uiStartMainHandle.Task;

			if(uiStartMainHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject uiStartMainGameObject = uiStartMainHandle.Result;
				uiStartMainGameObject.name = uiStartMainHandle.Result.name.Replace("(Clone)", string.Empty);
				UIVerticalManager.Instance.AttachUI(uiStartMainGameObject, uiStartMainHandle.Result.transform as RectTransform, UIVerticalManager.CanvasType.Main);
				UIStartMain = uiStartMainGameObject.GetComponent<UIStartMain>();
			}
		}

		private void Unload()
		{
			if(UIStartMain != null)
			{
				Addressables.ReleaseInstance(UIStartMain.gameObject);
				UIStartMain = null;
			}
		}

		private void Start()
		{
			//UIVerticalManager.Instance.FadeIn(0.5f, 0.5f);
			Application.targetFrameRate = 60;
			Application.runInBackground = true;

			GameDataManager.Instance.Load();
		}
	}
}
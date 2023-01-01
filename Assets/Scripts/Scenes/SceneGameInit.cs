using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class SceneGameInit : MonoBehaviourTemporarySingleton<SceneGameInit>
	{
		private async void Start()
		{
			Application.targetFrameRate = 60;
			Application.runInBackground = true;

			TableDataManager.Instance.DontDestroyOnLoad();
			TableDataManager.Instance.Load();
			await TableDataManager.Instance.Wait();
#if UNITY_EDITOR
			TableDataManager.Instance.Validate();
#endif

			SceneManager.LoadScene("SceneGameLobby", LoadSceneMode.Single);
		}
	}
}
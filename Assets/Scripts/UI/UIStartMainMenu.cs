using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectWilson.Lookups;
using ProjectWilson.Actions;
using UnityEngine.SceneManagement;

namespace ProjectWilson
{
	public class UIStartMainMenu : MonoBehaviour
	{
		public void OnClickARButton()
		{
			SceneManager.LoadScene("SceneAR", LoadSceneMode.Single);
		}

		public void OnClickGameButton()
		{
			SceneManager.LoadScene("SceneGameInit", LoadSceneMode.Single);
		}

		public void OnClickExit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
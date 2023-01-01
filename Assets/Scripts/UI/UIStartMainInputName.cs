using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

namespace ProjectWilson
{
	public class UIStartMainInputName : MonoBehaviour
	{
		[SerializeField]
		private TMP_InputField _PlayerNameInputField;
		private string _PlayerName;

		private void Awake()
		{
			Assert.IsNotNull<TMP_InputField>(_PlayerNameInputField);
		}

		private void OnEnable()
		{
			_PlayerNameInputField.text = string.Empty;
			_PlayerNameInputField.ActivateInputField();
			_PlayerName = string.Empty;
		}

		public void OnClickNext()
		{
			if(string.IsNullOrEmpty(_PlayerNameInputField.text))
				return;
			
			_PlayerName = _PlayerNameInputField.text;
			GameDataManager.Instance.Player.Name = _PlayerName;
			GameDataManager.Instance.Save();
			SceneStart.Instance.UIStartMain.SetMode(UIStartMain.Mode.Menu);
			Debug.LogWarning($"Player Name : {_PlayerName}");
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
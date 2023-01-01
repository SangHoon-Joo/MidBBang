using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace ProjectWilson
{
	public class UIGameLobbyMainSearchLobbyPopup : MonoBehaviour
	{
		[SerializeField]
		private AtlasImage _TimerImage;
		private float _Time;
		private float _MaxTime = 5f;
		public event System.Action OnCancel;

		private void Awake()
		{
			Assert.IsNotNull<AtlasImage>(_TimerImage);
		}

		private void OnEnable()
		{
			_Time = _MaxTime;
		}

		private void Update()
		{
			HandleTimer();
		}

		private void HandleTimer()
		{
			_Time -= Time.deltaTime;
			if (_Time < 0f)
			{
				TimeOut();
				return;
			}
			_TimerImage.fillAmount = 1f - (_Time / _MaxTime);
		}

		public void OnClickAccept()
		{
			SceneGameLobby.Instance.UIGameLobbyMain.SetMode(UIGameLobbyMain.Mode.InLobby);
			gameObject.SetActive(false);
		}

		public void OnClickCancel()
		{
			LobbyManager.Instance.LeaveLobby();
			gameObject.SetActive(false);
			OnCancel?.Invoke();
		}

		private void TimeOut()
		{
			OnClickCancel();
		}
	}
}
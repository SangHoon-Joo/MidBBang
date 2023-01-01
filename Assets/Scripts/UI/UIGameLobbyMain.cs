using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectWilson
{
	public class UIGameLobbyMain : MonoBehaviour
	{
		public enum Mode
		{
			Default,
			SearchLobby,
			InLobby,
			Max
		}

		private Mode _Mode = Mode.Default;
		private Mode _PreviousMode = Mode.Default;

		public UIGameLobbyMainSearchLobby UIGameLobbyMainSearchLobby;
		public UIGameLobbyMainInLobby UIGameLobbyMainInLobby;

		private void Awake()
		{
			Assert.IsNotNull<UIGameLobbyMainSearchLobby>(UIGameLobbyMainSearchLobby);
			Assert.IsNotNull<UIGameLobbyMainInLobby>(UIGameLobbyMainInLobby);
		}

		private void Start()
		{
			//SetMode(Mode.Default);
		}

		public void SetMode(Mode mode)
		{
			_PreviousMode = _Mode;
			_Mode = mode;

			UIGameLobbyMainSearchLobby.gameObject.SetActive(false);
			UIGameLobbyMainInLobby.gameObject.SetActive(false);

			switch(_Mode)
			{
				case Mode.Default:
					break;
				case Mode.SearchLobby:
					UIGameLobbyMainSearchLobby.gameObject.SetActive(true);
					break;
				case Mode.InLobby:
					UIGameLobbyMainInLobby.gameObject.SetActive(true);
					break;

			}
		}

		public Mode GetMode()
		{
			return _Mode;
		}
	}
}
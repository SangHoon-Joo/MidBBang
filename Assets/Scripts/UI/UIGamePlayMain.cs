using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectWilson
{
	public class UIGamePlayMain : MonoBehaviour
	{
		public enum Mode
		{
			Default,
			InPlay,
			Max
		}

		[SerializeField]
		private GameObject _GameOverVictory;
		[SerializeField]
		private GameObject _GameOverDefeat;
		private Mode _Mode = Mode.Default;
		private Mode _PreviousMode = Mode.Default;
		private NetworkPlayer _Player;

		public UIGamePlayMainJoystick UIGamePlayMainJoystick;

		private void Awake()
		{
			Assert.IsNotNull<UIGamePlayMainJoystick>(UIGamePlayMainJoystick);
			Assert.IsNotNull<GameObject>(_GameOverVictory);
			Assert.IsNotNull<GameObject>(_GameOverDefeat);
		}

		private void Start()
		{
			//SetMode(Mode.Default);
		}

		public void SetMode(Mode mode, NetworkPlayer player = null)
		{
			_PreviousMode = _Mode;
			_Mode = mode;
			_Player = player;

			UIGamePlayMainJoystick.gameObject.SetActive(false);

			switch(_Mode)
			{
				case Mode.Default:
					break;
				case Mode.InPlay:
					UIGamePlayMainJoystick.Init(player);
					UIGamePlayMainJoystick.gameObject.SetActive(true);
					break;
			}
		}

		public Mode GetMode()
		{
			return _Mode;
		}

		public void GameOver(bool isWin)
		{
			_GameOverVictory.SetActive(false);
			_GameOverDefeat.SetActive(false);
			if(isWin)
				_GameOverVictory.SetActive(true);
			else
				_GameOverDefeat.SetActive(true);
		}

		public void OnClickCameraTest()
		{
			if(CameraManager.Instance.GetMode() == CameraManager.Mode.Default)
				CameraManager.Instance.ChangeMode(CameraManager.Mode.GameOver);
			else
				CameraManager.Instance.ChangeMode(CameraManager.Mode.Default);
		}
	}
}
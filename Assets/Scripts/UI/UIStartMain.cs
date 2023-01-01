using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace ProjectWilson
{
	public class UIStartMain : MonoBehaviour
	{
		public enum Mode
		{
			Default,
			InputName,
			Menu,
			Max
		}

		private Mode _Mode = Mode.Default;
		private Mode _PreviousMode = Mode.Default;
		public UIStartMainInputName UIStartMainInputName;
		public UIStartMainMenu UIStartMainMenu;

		private void Awake()
		{
			Assert.IsNotNull<UIStartMainInputName>(UIStartMainInputName);
			Assert.IsNotNull<UIStartMainMenu>(UIStartMainMenu);
		}

		private void Start()
		{
			SetMode(Mode.InputName);
		}

		public void SetMode(Mode mode)
		{
			_PreviousMode = _Mode;
			_Mode = mode;

			UIStartMainInputName.gameObject.SetActive(false);
			UIStartMainMenu.gameObject.SetActive(false);

			switch(_Mode)
			{
				case Mode.Default:
					break;
				case Mode.InputName:
					UIStartMainInputName.gameObject.SetActive(true);
					break;
				case Mode.Menu:
					UIStartMainMenu.gameObject.SetActive(true);
					break;
			}
		}

		public Mode GetMode()
		{
			return _Mode;
		}
	}
}
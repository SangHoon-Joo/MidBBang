
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Krafton.SP2.X1.Lib
{
	public class InputFieldSelectEvent : MonoBehaviour, ISelectHandler
	{
		private Action _OnSelect;

		public void OnSelect(Action onSelect)
		{
			_OnSelect = onSelect;
		}

		public void OnSelect(BaseEventData data)
		{
			_OnSelect?.Invoke();
		}
	}
}

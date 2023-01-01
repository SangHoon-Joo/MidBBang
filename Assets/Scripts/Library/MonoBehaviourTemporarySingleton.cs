using System;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
	public abstract class MonoBehaviourTemporarySingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _Instance;
		public static T Instance
		{
			get { return _Instance; }
		}

		protected virtual void Awake()
		{
			_Instance = this as T;
		}

		protected virtual void OnDestroy()
		{
			_Instance = null;
		}
	}
}

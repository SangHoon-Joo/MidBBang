using System;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField]
		private bool _DontDestroyOnLoad = false;

		private static bool _IsApplicationQuit = false;

		private static Lazy<T> _LazyInstance = new Lazy<T>(LazyCreate);
		public static T Instance
		{
			get
			{
				if(_IsApplicationQuit)
				{
					Debug.LogWarning($"[Singleton] Instance {typeof(T).Name} already destroyed. Won't create again. Returning null.");
					return null;
				}
				return _LazyInstance.Value;
			}
		}

		private static T LazyCreate()
		{
			T instance = null;
			instance = FindObjectOfType<T>();
			if(instance == null)
			{
				GameObject gameObject = new GameObject();
				instance = gameObject.AddComponent<T>();
				instance.gameObject.name = $"{typeof(T).Name} (singleton)";
			}
			return instance;
		}

		protected virtual void Awake()
		{
			if(_DontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);

			Init();
		}

		public void DontDestroyOnLoad()
		{
			DontDestroyOnLoad(gameObject);
		}

		protected virtual void OnApplicationQuit()
		{
			_IsApplicationQuit = true;
		}

		protected virtual void OnDestroy()
		{
			Reset();
			if(!_IsApplicationQuit)
				_LazyInstance = new Lazy<T>(LazyCreate);
		}

		public virtual void Init()
		{
		}

		public virtual void Reset()
		{
		}
	}
}



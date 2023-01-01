using System;

namespace Krafton.SP2.X1.Lib
{
	public abstract class Singleton<T> where T : class, new()
	{
		private static readonly Lazy<T> _LazyInstance = new Lazy<T>(()=>new T());
		public static T Instance => _LazyInstance.Value;

		public virtual void Init()
		{
		}

		public virtual void Reset()
		{
		}
	}
}

using System.Collections;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
	public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>
	{
		public new Coroutine StartCoroutine(string methodName)
		{
			return base.StartCoroutine(methodName);
		}

		public new Coroutine StartCoroutine(IEnumerator routine)
		{
			return base.StartCoroutine(routine);
		}

		public new Coroutine StartCoroutine(string methodName, object value = null)
		{
			return base.StartCoroutine(methodName, value);
		}

		public new void StopAllCoroutines()
		{
			base.StopAllCoroutines();
		}

		public new void StopCoroutine(IEnumerator routine)
		{
			base.StopCoroutine(routine);
		}

		public new void StopCoroutine(Coroutine coroutine)
		{
			base.StopCoroutine(coroutine);
		}

		public new void StopCoroutine(string methodName)
		{
			base.StopCoroutine(methodName);
		}

		public Coroutine Delay(float seconds, System.Action action)
		{
			return base.StartCoroutine(WaitForSecondsCoroutine(seconds, action));
		}

		private IEnumerator WaitForSecondsCoroutine(float seconds, System.Action action)
		{
			yield return new WaitForSeconds(seconds);

			if(action != null)
				action();
		}

		public Coroutine DelayRealtime(float seconds, System.Action action)
		{
			return base.StartCoroutine(WaitForSecondsRealtimeCoroutine(seconds, action));
		}

		private IEnumerator WaitForSecondsRealtimeCoroutine(float seconds, System.Action action)
		{
			yield return new WaitForSecondsRealtime(seconds);

			if(action != null)
				action();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			StopAllCoroutines();
		}
	}
}

using System.Diagnostics;
using UnityEngine;

public static class Debug
{
	public static bool isDebugBuild
	{
		get { return UnityEngine.Debug.isDebugBuild; }
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void Log(object message, Object context)
	{
		UnityEngine.Debug.Log(message, context);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(context, format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogWarning(object message)
	{
		UnityEngine.Debug.LogWarning(message);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogWarning(object message, Object context)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogWarningFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogWarningFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(context, format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogError(object message)
	{
		UnityEngine.Debug.LogError(message);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogError(object message, Object context)
	{
		UnityEngine.Debug.LogError(message, context);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogErrorFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogErrorFormat(Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(context, format, args);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogException(System.Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void LogException(System.Exception exception, Object context)
	{
		UnityEngine.Debug.LogException(exception, context);
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void DrawLine( Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true )
	{
		UnityEngine.Debug.DrawLine( start, end, color, duration, depthTest );
	}

	[Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
	public static void DrawRay( Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true )
	{
		UnityEngine.Debug.DrawRay( start, dir, color, duration, depthTest );
	}
}
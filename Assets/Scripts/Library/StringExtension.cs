using System.Collections.Generic;
using UnityEngine;

public static class StringExtension
{
	public static void CopyToClipboard(this string str)
	{
		GUIUtility.systemCopyBuffer = str;
	}

	public static List<int> ParseInt(this string str, char separator)
	{
		List<int> ints = new List<int>();
		string[] intSubstrings = str.Split(separator);
		foreach(string intSubstring in intSubstrings)
		{
			int intResult;
			if(int.TryParse(intSubstring, out intResult))
			{
				ints.Add(intResult);
				continue;
			}
			float floatResult;
			if(float.TryParse(intSubstring, out floatResult))
			{
				ints.Add((int)floatResult);
				continue;
			}
		}
		return ints;
	}
	public static List<float> ParseFloat(this string str, char separator)
	{
		List<float> floats = new List<float>();
		string[] floatSubstrings = str.Split(separator);
		foreach(string floatSubstring in floatSubstrings)
		{
			float result;
			if(float.TryParse(floatSubstring, out result))
				floats.Add(result);
		}
		return floats;
	}

	public static Vector3 ParseVector3(this string str, char separator)
	{
		Vector3 vector = new Vector3();
		string[] floatSubstrings = str.Split(separator);
		for(int i = 0; i < floatSubstrings.Length; i++)
		{
			if(i > 2)
				break;

			string floatSubstring = floatSubstrings[i];
			float result;
			if(float.TryParse(floatSubstring, out result))
				vector[i] = result;
		}
		
		return vector;
	}

	public static List<string> ParseString(this string str, char separator)
	{
		List<string> strings = new List<string>();
		string[] substrings = str.Split(separator);
		foreach(string substring in substrings)
		{
			string result = substring.Trim();
			if(!string.IsNullOrEmpty(result))
				strings.Add(result);
		}
		return strings;
	}
}

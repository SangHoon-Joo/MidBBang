using System;
using System.Collections.Generic;

public static class ListExtension
{
	public static void Shuffle<T>(this IList<T> list)
	{
		Random random = new Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int j = random.Next(i + 1);
			T value = list[j];
			list[j] = list[i];
			list[i] = value;
		}
	}
}
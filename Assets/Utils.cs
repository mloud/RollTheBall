using UnityEngine;
using System.Collections.Generic;

public static class Utils 
{
	public static Vector2 ScreenSize()
	{
		return new Vector2(Screen.width, Screen.height);
	}

	public static List<T> FindAllDeep<T>(Transform tr) where T : MonoBehaviour
	{
		List<T> resuts = new List<T>();

		List<Transform> trList = new List<Transform>();

		trList.Add(tr);

		while (trList.Count > 0)
		{
			Transform firstTr = trList[0];

			resuts.AddRange(firstTr.gameObject.GetComponents<T>());
		
			for (int i = 0; i < firstTr.childCount; ++i)
			{
				trList.Add(firstTr.GetChild(i));
			}
		
			trList.RemoveAt(0);
		}

		return resuts;
	}

}
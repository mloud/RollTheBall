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


	public static Vector2 ScreenInWorldUnits ()
	{
		Camera cam = Camera.main;

		Vector3 p1 = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));  
		Vector3 p2 = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
		Vector3 p3 = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
		
		float width  = (p2 - p1).magnitude;
		float height = (p3 - p2).magnitude;
		
		return new Vector2 (width, height);
	}





}
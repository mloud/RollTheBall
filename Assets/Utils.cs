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

	public static void ClosestPoints(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, out Vector3 closestA, out Vector3 closestB)
	{
		float nA = Vector3.Dot(Vector3.Cross(b2 - b1, a1 - b1), Vector3.Cross(a2-a1,b2-b1));
		float nB = Vector3.Dot(Vector3.Cross(a2 - a1, a1 - b1), Vector3.Cross(a2-a1,b2-b1));
		float d = Vector3.Dot(Vector3.Cross(a2 - a1,b2 - b1),Vector3.Cross(a2 - a1, b2 - b1));

		closestA = a1 + (nA / d) * (a2 - a1);
		closestB = b1 + (nB / d) * (b2 - b1);
	}



}
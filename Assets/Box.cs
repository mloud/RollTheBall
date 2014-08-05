using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : MonoBehaviour
{
#if UNITY_EDITOR
	Box[] boxes;
#endif


#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Vector3 p1,p2;

		GetGizmosLinePoints(out p1, out p2);

		Gizmos.DrawLine(p1, p2);


		List<Vector3> closestPoints = GetClosestPointsToOtherBoxes ();
		for (int i = 0; i < closestPoints.Count; ++i)
		{
			Gizmos.DrawSphere(closestPoints[i], 0.2f);
		}
	}

	public void GetGizmosLinePoints(out Vector3 p1, out Vector3 p2)
	{
		float length = 8;
		
		Vector3 direction = transform.TransformDirection(Vector3.right);
		
		p1 = transform.position - direction * length;
		p2 = transform.position + direction * length;

	}

	List<Vector3> GetClosestPointsToOtherBoxes()
	{
		float closeDistance = 0.1f;

		List<Vector3> positions = new List<Vector3>();

		Box[] boxes = GameObject.FindObjectsOfType<Box>();

		for (int i = 0; i < boxes.Length; ++i)
		{
			if (boxes[i] != this)
			{
				Vector3 a1, a2;
				Vector3 b1, b2;


				this.GetGizmosLinePoints(out a1, out a2);
				boxes[i].GetGizmosLinePoints(out b1, out b2);

				Vector3 closestA;
				Vector3 closestB;

				Utils.ClosestPoints(a1, a2, b1, b2, out closestA, out closestB);

				if ( (closestA - closestB).sqrMagnitude < closeDistance * closeDistance)
				{
					positions.Add(closestA);
				}
			}
		}

		return positions;
	}



#endif
}

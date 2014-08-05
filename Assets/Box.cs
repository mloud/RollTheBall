using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		float length = 8;

		Vector3 direction = transform.TransformDirection(Vector3.right);

		Vector3 p1 = transform.position - direction * length;
		Vector3 p2 = transform.position + direction * length;

		Gizmos.DrawLine(p1, p2);
	}
}

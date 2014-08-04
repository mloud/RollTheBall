using UnityEngine;
using System.Collections;

public class Anchor : MonoBehaviour 
{
	[SerializeField]
	float offset;

	public enum Type
	{
		Left,
		Right,
		Top,
		Bottom
	}

	[SerializeField]
	Type anchorType;

	void Start ()
	{
		Vector3 pos = transform.position;
		if (anchorType == Type.Left)
		{
			pos.x = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
			pos.x += offset;
		}
		else if (anchorType == Type.Right)
		{
			pos.x = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane)).x;
			pos.x -= offset;
		}
		else if (anchorType == Type.Top)
		{
			pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;
			pos.y -= offset;
		}
		else if (anchorType == Type.Bottom)
		{
			pos.y = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;
			pos.y += offset;
		}

		transform.position = pos;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

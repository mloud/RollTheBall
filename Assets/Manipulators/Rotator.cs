using UnityEngine;
using System.Collections;

public class Rotator : Manipulator 
{
	[SerializeField]
	float speed = 200.0f;


	private Vector3 axeToRotate;
	private float angleToRotate;

	void Update ()
	{
		if (Running)
		{
			float angle = Time.deltaTime * speed * Mathf.Sign(angleToRotate);

			if (Mathf.Abs(angleToRotate) >= Mathf.Abs(angle))
			{
				transform.parent.RotateAround(transformToManipulate.position,  axeToRotate, angle);
				angleToRotate -= angle;
			}
			else
			{
				angle = angleToRotate;
				transform.parent.RotateAround(transformToManipulate.position,  axeToRotate, angle);
				Running = false;

				Value *= -1;
			}
		}
	}

	
	public override void Manipulate()
	{
		if (!Running)
		{

			axeToRotate = new Vector3(ActiveAxe == Axe.x ? 1 : 0,
			                          ActiveAxe == Axe.y ? 1 : 0,
			                          ActiveAxe == Axe.z ? 1 : 0);

			Running = true;
			angleToRotate = Value;
		}
	}

}

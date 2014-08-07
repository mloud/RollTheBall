using UnityEngine;
using System.Collections;

public class Rotator : Manipulator 
{
	[SerializeField]
	float speed = 200.0f;


	[SerializeField]
	RotationAround RotateAround;

	public enum RotationAround
	{
		x = 0,
		y, 
		z
	}

	[SerializeField]
	float OnClickRotation;

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

				OnClickRotation *= -1;
			}


		}
	}

	
	public override void Manipulate()
	{
		if (RotateAround == RotationAround.x)
		{
			axeToRotate = new Vector3(1,0,0);
		}
		else if (RotateAround == RotationAround.y)
		{
			axeToRotate = new Vector3(0,1,0);
		}
		else if (RotateAround == RotationAround.z)
		{
			axeToRotate = new Vector3(0,0,1);
		}
		Running = true;
		angleToRotate = OnClickRotation;
	}

}

using UnityEngine;
using System.Collections;

public class Rotator : Manipulator 
{

	[SerializeField]
	Vector3 OnClickRotation;

	private Vector3? RotateAround;
	float angleToRotate;

	void Update ()
	{
		if (RotateAround != null)
		{
			float angle = Time.deltaTime * 60.0f * Mathf.Sign(angleToRotate);

			if (Mathf.Abs(angleToRotate) >= Mathf.Abs(angle))
			{
				transform.parent.RotateAround(transformToManipulate.position,  RotateAround.Value, angle);
				angleToRotate -= angle;
			}
			else
			{
				angle = angleToRotate;
				transform.parent.RotateAround(transformToManipulate.position,  RotateAround.Value, angle);
				RotateAround = null;
			}


		}
	}

	
	public override void Manipulate()
	{
		if (OnClickRotation.x != 0)
		{
			//transform.parent.RotateAround(transform.position,  new Vector3(1,0,0), OnClickRotation.x);
			RotateAround = new Vector3(1,0,0);
			angleToRotate = OnClickRotation.x;
		}

		if (OnClickRotation.y != 0)
		{
			//transform.parent.RotateAround(transform.position, new Vector3(0,1,0), OnClickRotation.y);
			RotateAround = new Vector3(0,1,0);
			angleToRotate = OnClickRotation.y;
		}

		if (OnClickRotation.z != 0)
		{
			//transform.parent.RotateAround(transform.position, new Vector3(0,0,1), OnClickRotation.z);
			RotateAround = new Vector3(0,0,1);
			angleToRotate = OnClickRotation.z;
		}
	}

}

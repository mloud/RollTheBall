using UnityEngine;
using System.Collections;

public class Translator : Manipulator
{
	[SerializeField]
	float speed = 200.0f;

	[SerializeField]
	Space space = Space.Self;

	private Vector3 axeToTranslate;
	private float distance;

	void Start ()
	{
		
	}
	

	void Update ()
	{
		if (Running)
		{
			float distanceStep = Time.deltaTime * speed * Mathf.Sign(Value);
			
			if (Mathf.Abs(distance) >= Mathf.Abs(distanceStep))
			{
				transform.parent.Translate(axeToTranslate * distanceStep, space);

				distance -= distanceStep;
			}
			else
			{
				distanceStep = distance;
				transform.parent.Translate(axeToTranslate * distanceStep);
				Running = false;
				
				Value *= -1;
			}
		}
	}


	public override void Manipulate()
	{
		if (!Running)
		{
			axeToTranslate = new Vector3(ActiveAxe == Axe.x ? 1 : 0,
			                             ActiveAxe == Axe.y ? 1 : 0,
			                             ActiveAxe == Axe.z ? 1 : 0);


		
			Running = true;
			distance = Value;
		}
	}
}

using UnityEngine;
using System.Collections;

public abstract class Manipulator : MonoBehaviour 
{
	[SerializeField]
	protected Transform transformToManipulate;

	[SerializeField]
	protected Axe ActiveAxe;

	[SerializeField]
	protected float Value;


	public enum Axe
	{
		x = 0,
		y, 
		z
	}


	protected bool Running { get; set; }

	void Start()
	{
		if (transformToManipulate == null)
		{
			transformToManipulate = transform.parent;
		}
	}

	public abstract void Manipulate();
}

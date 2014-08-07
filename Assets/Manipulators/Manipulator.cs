using UnityEngine;
using System.Collections;

public abstract class Manipulator : MonoBehaviour 
{
	[SerializeField]
	protected Transform transformToManipulate;

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

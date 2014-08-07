using UnityEngine;
using System.Collections;

public abstract class Manipulator : MonoBehaviour 
{
	[SerializeField]
	protected Transform transformToManipulate;

	void Start()
	{
		if (transformToManipulate == null)
		{
			transformToManipulate = transform.parent;
		}
	}

	public abstract void Manipulate();
}

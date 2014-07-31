using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour 
{
	[SerializeField]
	public bool Active;

	public Segment Segment { get; private set; }

	void Start ()
	{
		Segment = transform.parent.parent.gameObject.GetComponent<Segment>();
	}
	
	void Update () {
	
	}
}

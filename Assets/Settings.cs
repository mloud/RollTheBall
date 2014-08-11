using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
	public static Settings Instance { get { return _instance; }} 


	private static Settings _instance;

	void Awake()
	{
		_instance = this;
	}

	[SerializeField]
	public float TresholdDistance = 15; 

	[SerializeField]
	public bool ApplySegmentShift = false;


	
}

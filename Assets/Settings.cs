using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	[SerializeField]
	public float BottomReactionArea;

	[SerializeField]
	public float RightReactionArea;


	public static Settings Instance { get { return _instance; }}
	
	private static Settings _instance;

	void Awake ()
	{
		_instance = this;	
	}
	

}

using UnityEngine;
using System.Collections;

public class MUI : MonoBehaviour
{
	[SerializeField]
	public Camera UICamera;

	[SerializeField]
	GameObject finish;

	public static MUI Instance { get { return _instance; }}
	
	private static MUI _instance;

	void Awake()
	{
		_instance = this;
	}

	public void ShowFinishGame()
	{
		GameObject finishGo = Instantiate(finish) as GameObject;

		finishGo.transform.parent = transform;
		finishGo.transform.localEulerAngles = Vector3.zero;
		finishGo.transform.localPosition = Vector3.zero;
	}


}

using UnityEngine;
using System.Collections.Generic;

public class MUI : MonoBehaviour
{
	[SerializeField]
	public Camera UICamera;

	[SerializeField]
	GameObject finish;

	[SerializeField]
	List<GameObject> Arrows;


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

	public void HideArrows()
	{
		for (int i = 0; i < Arrows.Count; ++i)
		{
			Arrows[i].gameObject.SetActive(false);
		}
	}



}

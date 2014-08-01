using UnityEngine;
using System.Collections;

public class MUI : MonoBehaviour
{

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
		finish.SetActive(true);
	}


}

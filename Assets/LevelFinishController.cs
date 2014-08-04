using UnityEngine;
using System.Collections;

public class LevelFinishController : MonoBehaviour {

	[SerializeField]
	GameObject btnNext;

	[SerializeField]
	GameObject btnReplay;


	// Use this for initialization
	void Start () 
	{
		// is another level
		if (Application.loadedLevel == Application.levelCount - 1)
		{
			btnNext.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

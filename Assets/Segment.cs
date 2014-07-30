using UnityEngine;
using System.Collections.Generic;

public class Segment : MonoBehaviour
{
	[SerializeField]
	public List<Transform> Connectors;


	private List<GameObject> PipeList { get; set; }


	private void Awake()
	{
		PipeList = new List<GameObject>();

		for (int i = 0; i < gameObject.transform.childCount; ++i)
		{
			PipeList.Add(gameObject.transform.GetChild(i).gameObject);
		}
	}

	public void Highlight(bool highlight)
	{
		for (int i = 0; i < PipeList.Count; ++i)
		{
			PipeList[i].gameObject.renderer.material.color = highlight ? Color.red :  Color.white;
		}
	}

	void Update ()
	{
	
	}
}

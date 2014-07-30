using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	[SerializeField]
	List<Segment> Segments;

	[SerializeField]
	Transform root;

	SceneController SceneController { get; set; }


	void Start()
	{
		SceneController = new SceneController();
		SceneController.RootPoint = root;
	}

	void OnDestroy()
	{
		SceneController.Release();
	}


	void Update () 
	{
		for (int i = 0; i < Segments.Count; ++i)
		{
			Segments[i].Highlight(false);
		}
	
		for (int i = 0; i < Segments.Count; ++i)
		{
			for (int j = i + 1; j < Segments.Count; ++j)
			{
				if (SegmentConnected(Segments[i], Segments[j]))
				{
					Segments[i].Highlight(true);
					Segments[j].Highlight(true);
				}
			}
		}
	}


	bool SegmentConnected(Segment seg1, Segment seg2)
	{
		for (int i = 0; i < seg1.Connectors.Count; ++i)
		{
			for (int j = 0; j < seg2.Connectors.Count; ++j)
			{
				//project points to screenspace - if in touch, they are connected
				Vector3 pos1 = Camera.main.WorldToScreenPoint(seg1.Connectors[i].transform.position);
				Vector3 pos2 = Camera.main.WorldToScreenPoint(seg2.Connectors[j].transform.position);

				if ( (pos1 - pos2).magnitude < 10.0f)
				{
					return true;
				}
			}
		}

		return false;
	}





}

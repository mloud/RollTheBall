using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	[SerializeField]
	List<Segment> Segments;

	
	void Update () 
	{
		for (int i = 0; i < Segments.Count; ++i)
		{
			for (int j = i + 1; j < Segments.Count; ++j)
			{


			}
		}
	}


	bool SegmetConnected(Segment seg1, Segment seg2)
	{

		for (int i = 0; i < seg1.Connectors.Count; ++i)
		{
			for (int j = 0; j < seg2.Connectors.Count; ++j)
			{
				//project points to screenspace - if in touch, they are connected
				
			}

		}
	}

}

using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	[SerializeField]
	List<Segment> Segments;

	[SerializeField]
	Transform root;

	[SerializeField]
	Ball ball;

	[SerializeField]
	Segment BallStartSegment;

	[SerializeField]
	Waypoint BallStartWaypoint;


	[SerializeField]
	Transform BallFinish;

	public static Game Instance { get { return _instance; }}

	private static Game _instance;

	SceneController SceneController { get; set; }


	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		SceneController = new SceneController();
		SceneController.RootPoint = root;


		ball.PlaceToSegment(BallStartSegment, BallStartWaypoint);
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
				if (IsSegmentConnected(Segments[i], Segments[j]))
				{
					Segments[i].Highlight(true);
					Segments[j].Highlight(true);
				}
			}
		}
	}


	public Segment FindConnectedSegment(Connector inputSegConnector, Segment inputSegment)
	{
		for (int i = 0; i < Segments.Count; ++i)
		{
			if (Segments[i] != inputSegment)
			{
				if (IsSegmentConnected(inputSegment, Segments[i], inputSegConnector, null))
				{
					return Segments[i];
				}
			}
		}
		
		return null;
	}

	public Segment GetConnectedSegment(Connector connector)
	{
		for (int i = 0; i < Segments.Count; ++i)
		{
			if (Segments[i]  != connector.Segment)
			{
				for (int j = 0; j <  Segments[i].Connectors.Count; ++j)
				{
					Connector otherConn = Segments[i].Connectors[j];

					if (otherConn.Active)
					{
						//project points to screenspace - if in touch, they are connected
						Vector3 pos1 = Camera.main.WorldToScreenPoint(otherConn.transform.position);
						Vector3 pos2 = Camera.main.WorldToScreenPoint(connector.transform.position);
						
						if ( (pos1 - pos2).magnitude < 10.0f)
						{
							return Segments[i];
						}
					}
				}
			}
		}

		return null;
	}


	public bool IsSegmentConnected(Segment seg1, Segment seg2, Connector seg1Connector = null,  Connector seg2Connector = null)
	{
		for (int i = 0; i < seg1.Connectors.Count; ++i)
		{
			Connector connector1  = seg1.Connectors[i];

			if ( (seg1Connector != null && connector1 != seg1Connector) || !connector1.Active)
				continue;
			
			for (int j = 0; j < seg2.Connectors.Count; ++j)
			{

				if (seg2Connector != null && seg2.Connectors[j] != seg2Connector)
					continue;
				

				Connector connector2  = seg2.Connectors[j];
				
				if (connector2.Active)
				{
					
					//project points to screenspace - if in touch, they are connected
					Vector3 pos1 = Camera.main.WorldToScreenPoint(connector1.transform.position);
					Vector3 pos2 = Camera.main.WorldToScreenPoint(connector2.transform.position);
					
					if ( (pos1 - pos2).magnitude < 10.0f)
					{
						return true;
					}
				}
			}
		}
		
		return false;
	}





}

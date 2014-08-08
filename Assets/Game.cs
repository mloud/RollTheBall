using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour, UI.IObjectHitListener
{
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

	[SerializeField]
	float SegmentConnectedDistance;


	public static Game Instance { get { return _instance; }}

	private static Game _instance;

	private SceneController SceneController { get; set; }

	private List<Segment> Segments { get; set; }


	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		UI.TouchManager.Instance.RegisterObjectHitListener(this);

		Segments = new List<Segment>(Utils.FindAllDeep<Segment>(root));

		SceneController = new SceneController();
		SceneController.RootPoint = root;


		ball.PlaceToSegment(BallStartSegment, BallStartWaypoint);

	}

	void OnDestroy()
	{
		SceneController.Release();

		UI.TouchManager.Instance.UntegisterObjectHitListener(this);
	}


	void Update () 
	{
		SceneController.Update();

		HighlightConnectedSegments();
	}


	public void HighlightConnectedSegments()
	{
		for (int i = 0; i < Segments.Count; ++i)
		{
			Segments[i].Highlight(false);
		}
		
		for (int i = 0; i < Segments.Count; ++i)
		{
			for (int j = i + 1; j < Segments.Count; ++j)
			{
				if (Utils.IsSegmentConnected(Camera.main, Segments[i], Segments[j], SegmentConnectedDistance) != null)
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
				if (Utils.IsSegmentConnected(Camera.main, inputSegment, Segments[i], SegmentConnectedDistance, inputSegConnector, null) != null)
				{
					return Segments[i];
				}
			}
		}
		
		return null;
	}

	public List<Connector> GetConnectedSegments(Connector connector)
	{
		List<Connector> connectedConnectors = new List<Connector>();

		Vector3 pos2 = Camera.main.WorldToScreenPoint(connector.transform.position);

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
					
						
						if ( (pos1 - pos2).magnitude < SegmentConnectedDistance)
						{
							connectedConnectors.Add(otherConn);
						}
					}
				}
			}
		}

		return connectedConnectors;
	}




	public void ObjectHit(GameObject obj)
	{
		if (obj.name ==  "BtnNext")
		{
			LoadNextLevel();
		}
		else if (obj.name ==  "BtnReplay")
		{
			Replay();
		}
		else if (obj.name == "BtnRestart")
		{
			Replay ();
		}
	}


	void LoadNextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	void Replay()
	{
	
		Application.LoadLevel(Application.loadedLevel);
	}


	void RestartLevel()
	{

	}




}

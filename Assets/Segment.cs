using UnityEngine;
using System.Collections.Generic;



public class Segment : MonoBehaviour
{
	public List<Connector> Connectors 
	{
		get { 
			if (_connectors == null)
			{
				MakeListOfPipes();
				MakeListOfConnectors();
			}
			return _connectors;
		}

		private set { _connectors = value; }
	}


	public List<Waypoint> Waypoints { get; set; }

	private List<GameObject> PipeList { get; set; }
	private List<Manipulator> Manipulators { get; set; }
	private List<Connector> _connectors;


	private Vector3 _originalPos { get; set; }
	private bool Shifted { get; set; }



	class OrthoVisual
	{
		public Renderer renderer;
		public Transform origTransform;
	}

	private List<OrthoVisual> OrthoVisuals { get; set; }
	private GameObject Visual { get; set; }


	private class Connection
	{
		public Connector ownConnector { get; set; }
		public Connector otherConnector { get; set; }
	}


	private Connection ActiveConnection { get; set; }

	private void Awake()
	{
		_originalPos = transform.position;

		MakeListOfPipes();
		MakeListOfConnectors();
		MakeListOfWaypoints();
		MakeListOfManipulators();

		CreateOrthoVisuals();
	
	
		Visual = transform.FindChild("Box").gameObject;
	}

	private void CreateOrthoVisuals()
	{
		OrthoVisuals = new List<OrthoVisual>();
		Box[] boxes = gameObject.transform.GetComponentsInChildren<Box>();
		
		for (int i = 0; i < boxes.Length; ++i)
		{
			GameObject copy = Instantiate(boxes[i].gameObject, boxes[i].transform.position, boxes[i].transform.rotation) as GameObject;
			copy.transform.parent =  boxes[i].transform.parent;
			copy.transform.localScale = boxes[i].transform.localScale;
			
			OrthoVisual orthoVis = new OrthoVisual();
			orthoVis.origTransform = boxes[i].gameObject.transform;
			orthoVis.renderer = copy.renderer;

			Destroy(copy.collider);
			Destroy(copy.GetComponent<Box>());

			for (int j = 0; j < copy.transform.childCount; ++j)
			{
				Destroy (copy.transform.GetChild(j).gameObject);
			}


			OrthoVisuals.Add(orthoVis);
			copy.renderer.enabled = false;
		}
	}


	// Creates list of all pipes in segment
	private void MakeListOfPipes()
	{
		PipeList = new List<GameObject>();
		
		for (int i = 0; i < gameObject.transform.childCount; ++i)
		{
			PipeList.Add(gameObject.transform.GetChild(i).gameObject);
		}
	}

	private void MakeListOfConnectors()
	{
		Connectors = new List<Connector>();

		for (int i = 0; i < PipeList.Count; ++i)
		{
			Connector[] connectors = PipeList[i].GetComponentsInChildren<Connector>();
			Connectors.AddRange(connectors);
		}
	}

	private void MakeListOfWaypoints()
	{
		Waypoints = new List<Waypoint>();

		for (int i = 0; i < PipeList.Count; ++i)
		{
			Waypoint[] waypoints = PipeList[i].GetComponentsInChildren<Waypoint>();
			Waypoints.AddRange(waypoints);
		}

	}

	private void MakeListOfManipulators()
	{
		Manipulators = new List<Manipulator>(transform.GetComponentsInChildren<Manipulator>());
	}


		
	public List<Connector> GetSortedConnectors(Vector3 pos)
	{
		List<Connector> sortedConnectors = new List<Connector>(Connectors);

		sortedConnectors.Sort(delegate(Connector conn1, Connector conn2)
		{
			float sqrtDistWp1 = (pos - conn1.transform.position).sqrMagnitude;
			float sqrtDistWp2 = (pos - conn2.transform.position).sqrMagnitude;
			return sqrtDistWp1 < sqrtDistWp2 ? -1 : 1;
		});
		
		return sortedConnectors;
	}

	public List<Waypoint> GetSortedWaypoints(Vector3 pos)
	{
		List<Waypoint> sortedListByDistance = new List<Waypoint>(Waypoints);

		sortedListByDistance.Sort(delegate(Waypoint wp1, Waypoint wp2)
		{
			float sqrtDistWp1 = (pos - wp1.transform.position).sqrMagnitude;
			float sqrtDistWp2 = (pos - wp2.transform.position).sqrMagnitude;
			return sqrtDistWp1 < sqrtDistWp2 ? -1 : 1;
		});

		return sortedListByDistance;
	}


	public void Highlight(bool highlight)
	{
		for (int i = 0; i < PipeList.Count; ++i)
		{
			PipeList[i].gameObject.renderer.material.color = highlight ? Color.red :  Color.white;
		}
	}

	void Update()
	{

		if (ActiveConnection != null)
		{
			if (Utils.IsConnectorsConnected(Camera.main, ActiveConnection.ownConnector, ActiveConnection.otherConnector))
			{

			}
			else
			{
				ActiveConnection = null;

				for (int i = 0; i < OrthoVisuals.Count;++i)
				{
					OrthoVisuals[i].renderer.enabled = false;
				}
				Visual.renderer.enabled = true;
			}
		}
	
	}




	public void OnTouch()
	{
		for (int i = 0; i < Manipulators.Count; ++i)
		{
			Game.Instance.PlayerStatus.Moves++;
			Manipulators[i].Manipulate();
		}
	}



	public void ConnectTo(Connector ownConnector, Connector otherConnector)
	{
		if (ActiveConnection == null)
		{
			ActiveConnection = new Connection();

			ActiveConnection.ownConnector = ownConnector;
			ActiveConnection.otherConnector = otherConnector;

			float dist1 = (Camera.main.transform.position - ownConnector.transform.position).magnitude;
			float dist2 = (Camera.main.transform.position - otherConnector.transform.position).magnitude;

			if (dist1 > dist2)
			{

				for (int i = 0; i < OrthoVisuals.Count; ++i)
				{
					OrthoVisuals[i].renderer.enabled = true;
					OrthoVisuals[i].renderer.gameObject.transform.position = OrthoVisuals[i].origTransform.position;
					OrthoVisuals[i].renderer.gameObject.transform.rotation = OrthoVisuals[i].origTransform.rotation;
					OrthoVisuals[i].renderer.gameObject.transform.Translate( Utils.ComputeCameraNormal(Camera.main).normalized * Mathf.Abs(dist1 - dist2),Space.World);
				}
			
				Visual.renderer.enabled = false;
			}

		}
	}





}

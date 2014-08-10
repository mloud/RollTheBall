using UnityEngine;
using System.Collections.Generic;

public class CrossRoad : MonoBehaviour 
{
	[SerializeField]
	GameObject arrowPrefab;

	public Connector CurrentConnector { get; private set; }

	private List<CrossRoadArrow> Arrows { get; set; }

	private List<Connector> Connectors { get; set; }

	private Connector InputConnector { get; set; }


	void Awake()
	{
		Arrows = new List<CrossRoadArrow>();
		Connectors = new List<Connector>();
	}


	public void Init(Connector inputConnector)
	{
		// Clear old arrows
		for (int i = 0; i < Arrows.Count; ++i)
		{
			Destroy(Arrows[i].gameObject);
		}
		Arrows.Clear();

		InputConnector = inputConnector;
	

		gameObject.transform.position = inputConnector.transform.position;


		// set 0 as current connector
		SwitchSegment(); 
	}

	public void SwitchSegment()
	{
		if (CurrentConnector == null && Connectors.Count > 0)
		{
			CurrentConnector = Connectors[0];
		}
		else if (Connectors.Count > 0)
		{
			int index = Connectors.FindIndex(x=> x == CurrentConnector);

			if (index == -1)
			{
				CurrentConnector = Connectors[0];
			}
			else
			{
				++index;
				if (index >= Connectors.Count)
				{
					index = 0;
				}

				CurrentConnector = Connectors[index];
			}
		}
		else
		{
			CurrentConnector = null;
		}
	}


	private void UpdateArrows()
	{
		// remove invalid arrows
		int removeArrowsCount = Arrows.Count - Connectors.Count;
		for (int i = 0; i < removeArrowsCount; ++i)
		{
			Destroy(Arrows[i].transform.parent.gameObject);
		}

		if (removeArrowsCount > 0)
			Arrows.RemoveRange(0, removeArrowsCount);

		// add new arrows
		int arrowsToAdd = removeArrowsCount < 0 ? Mathf.Abs(removeArrowsCount) : 0;

		for (int i = 0; i < arrowsToAdd; ++i)
		{
			GameObject arrowGo = Instantiate(arrowPrefab) as GameObject;
			arrowGo.transform.parent = transform;
			Arrows.Add (arrowGo.GetComponentInChildren<CrossRoadArrow>());
			Arrows[i].CrossRoad = this;
		}



		for (int i = 0; i < Connectors.Count; ++i)
		{
			List<Waypoint> waypoints = Connectors[i].Segment.GetSortedWaypoints(Connectors[i].gameObject.transform.position);
			
			Vector3 waypointVec = waypoints[1].transform.position - waypoints[0].transform.position;
			
			Arrows[i].transform.parent.parent = Connectors[i].Segment.transform;
			// Arrows[i].transform.parent.position ->parent is container
			Arrows[i].transform.parent.position = waypoints[0].transform.position + waypointVec * 0.5f;
			Arrows[i].transform.parent.rotation = Quaternion.LookRotation(waypointVec);
			
			if (Connectors[i] != CurrentConnector)
			{
				Arrows[i].Disable();
			}
			else
			{
				Arrows[i].Enable();
			}
		}
	}


	void OnDestroy()
	{
		for (int i = 0; i < Arrows.Count; ++i)
		{
			Destroy(Arrows[i].transform.parent.gameObject);
		}
	}

	void UpdateConnectors()
	{

		// check all connectors for overlapps
		List<Connector> connectedConnectors = Game.Instance.GetConnectedSegments(InputConnector);
		
		
		for (int i = 0; i < Connectors.Count; ++i)
		{
			int index = connectedConnectors.FindIndex(x=>x == Connectors[i]);
			
			if (index == -1)
			{
				Connectors[i] = null;
			}
			else
			{
				connectedConnectors.Remove(Connectors[i]); // already exist
			}
		}

		Connectors.RemoveAll(x=>x == null);


		for (int i = 0; i < connectedConnectors.Count; ++i)
		{
			Connectors.Add (connectedConnectors[i]);
		}


		if (Connectors.Find(x => x == CurrentConnector) == null)
		{
			CurrentConnector = Connectors.Count > 0 ? Connectors[0] : null;
		}

	}


	void Update ()
	{
		// connectors
		UpdateConnectors();

		// update arrows
		UpdateArrows();

	}
}

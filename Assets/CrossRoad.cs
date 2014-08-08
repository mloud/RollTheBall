using UnityEngine;
using System.Collections.Generic;

public class CrossRoad : MonoBehaviour 
{
	[SerializeField]
	GameObject arrowPrefab;



	private List<CrossRoadArrow> Arrows { get; set; }

	private List<Connector> Connectors { get; set; }

	private int CurrentConnectorIndex { get; set; }

	void Awake()
	{
		Arrows = new List<CrossRoadArrow>();
	}


	public void Init(List<Connector> connectors)
	{
		// Clear old arrows
		for (int i = 0; i < Arrows.Count; ++i)
		{
			Destroy(Arrows[i].gameObject);
		}
		Arrows.Clear();


		Connectors = connectors;
		CurrentConnectorIndex = 0;
	
		// connectors overlaps - choose the first as position

		// remove unused arrows - number of arrows a connectors is equal

		for (int i = 0; i < connectors.Count; ++i)
		{
			GameObject arrowGo = Instantiate(arrowPrefab) as GameObject;
			arrowGo.transform.parent = transform;
			Arrows.Add (arrowGo.GetComponentInChildren<CrossRoadArrow>());
			Arrows[i].CrossRoad = this;
		}

	

		gameObject.transform.position = Connectors[0].transform.position;


		// set 0 as current connector
		CurrentConnectorIndex = -1;
		SwitchSegment(); 
	}

	public void SwitchSegment()
	{
		CurrentConnectorIndex++;

		if (CurrentConnectorIndex == Connectors.Count)
		{
			CurrentConnectorIndex = 0;
		}

		// make arrow to point from waypoint-0 to waypoint-1


		for (int i = 0; i < Connectors.Count; ++i)
		{
			List<Waypoint> waypoints = Connectors[i].Segment.GetSortedWaypoints(Connectors[i].gameObject.transform.position);

			Vector3 waypointVec = waypoints[1].transform.position - waypoints[0].transform.position;

			Arrows[i].transform.parent.parent = Connectors[i].Segment.transform;
			// Arrows[i].transform.parent.position ->parent is container
			Arrows[i].transform.parent.position = waypoints[0].transform.position + waypointVec * 0.5f;
			Arrows[i].transform.parent.rotation = Quaternion.LookRotation(waypointVec);
		
			if (i != CurrentConnectorIndex)
			{
				Arrows[i].Disable();
			}
			else
			{
				Arrows[i].Enable();
			}
		}
	}

	public Connector GetCurrentConnector()
	{
		return Connectors[CurrentConnectorIndex];
	}

	void OnDestroy()
	{
		for (int i = 0; i < Arrows.Count; ++i)
		{
			Destroy(Arrows[i].transform.parent.gameObject);
		}
	}

	void Update ()
	{
	
	}
}

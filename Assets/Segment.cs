using UnityEngine;
using System.Collections.Generic;

public class Segment : MonoBehaviour
{
	public List<Connector> Connectors { get; set; }

	private List<GameObject> PipeList { get; set; }

	public List<Waypoint> Waypoints { get; set; }


	private void Awake()
	{
		MakeListOfPipes();
		MakeListOfConnectors();
		MakeListOfWaypoints();
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

	void Update ()
	{
	
	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
	[SerializeField]
	GameObject crossRoadPrefab;

	[SerializeField]
	float speed;

	private const float DESTINATION_TRESHOLD = 0.01f;

	private Segment CurrentSegment { get; set; }

	private Waypoint CurrentWaypoint { get; set; }

	private List<Waypoint> DestinationWaypoints { get; set; }

	private JumpState Jump { get; set; }

	private CrossRoad CrossRoad { get; set; }

	class JumpState
	{
		public Waypoint DestinationWaypoint;
		public Segment DestinationSegment;
	}


	public enum State
	{
		Static,
		Moving,
		Jumping,
		WaitingCrossRoad
	}

	public State CurrentState { get; set; }

	public void PlaceToSegment(Segment segment, Waypoint waypoint)
	{
		CurrentSegment = segment;
		CurrentWaypoint = waypoint;

		gameObject.transform.position = waypoint.transform.position;
	}


	void Start()
	{
		CurrentState = State.Static;
	}

	void Update () 
	{
		switch (CurrentState)
		{
		case State.Moving:
			UpdateMoving();
			break;
		
		case State.Jumping:
			UpdateJumping();
			break;

		case State.WaitingCrossRoad:
			UpdateWaitingCrossRoad();
			break;
		}
	}

	void OnEndSegmentReached()
	{
		Debug.Log("Ball.OnEndSegmentReached() " + CurrentSegment.name);

		List<Connector> nearestConnectors = CurrentSegment.GetSortedConnectors(transform.position);

		Connector nearestConnectorOnSegment = nearestConnectors[0];

		Debug.Log ("Ball.OnEndSegmentReached() - nearest connector " + nearestConnectorOnSegment.name);

		List<Connector> connectedConnectors = Game.Instance.GetConnectedSegments(nearestConnectorOnSegment);


		// just one segment connected
		if (connectedConnectors.Count == 1)
		{
			JumpToSegment(connectedConnectors[0].Segment);
		}
		// Make joice which segment
		else if (connectedConnectors.Count > 1)
		{
			WaitAtCrossRoad(nearestConnectorOnSegment, connectedConnectors);
		}
	}

	void OnEndJumpFinished()
	{

		CurrentSegment = Jump.DestinationSegment;
		CurrentWaypoint = Jump.DestinationWaypoint;

		MoveFromWaypoint();

		Jump = null;
	}


	void UpdateJumping()
	{
		Vector3 dstScreenPos = Camera.main.WorldToScreenPoint(Jump.DestinationWaypoint.transform.position);

		Vector3 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 newScreenPos = Vector3.Lerp(currentScreenPos, dstScreenPos, Time.deltaTime * speed);

		if ( (newScreenPos - dstScreenPos).magnitude < 10)
		{
			transform.position = Jump.DestinationWaypoint.transform.position;

			OnEndJumpFinished();

		}
		else
		{
			transform.position = Camera.main.ScreenToWorldPoint(newScreenPos);
		}

	}

	void UpdateMoving()
	{
		if (DestinationWaypoints !=  null)
		{
			Waypoint nextWaypoint = DestinationWaypoints[0];
			
			transform.position = Vector3.Lerp(transform.position, nextWaypoint.transform.position, Time.deltaTime * speed);
			
			//we are at the end
			if ( (transform.position - nextWaypoint.transform.position).sqrMagnitude < DESTINATION_TRESHOLD * DESTINATION_TRESHOLD)
			{
				transform.position = nextWaypoint.transform.position;
				
				CurrentWaypoint = nextWaypoint;
				
				DestinationWaypoints.RemoveAt(0);
				
				if (DestinationWaypoints.Count == 0)
				{
					DestinationWaypoints = null;


					CurrentState = State.Static;


					OnEndSegmentReached();
				}
			}
		}
	}

	void UpdateWaitingCrossRoad()
	{

	}

	void JumpToSegment(Segment segment)
	{
		Debug.Log("Ball.JumpToSegment() " + segment.name);

		CurrentState = State.Jumping;

		Vector3 currentScreenCoord = Camera.main.WorldToScreenPoint(transform.position);

		Jump = new JumpState();

		Jump.DestinationSegment = segment;
	
		// start - end waypoint
		Waypoint wp1 = segment.Waypoints[0];
		Waypoint wp2 = segment.Waypoints[ segment.Waypoints.Count - 1];

			

		// select nearest in screen coord
		float screenDist1 = (currentScreenCoord - Camera.main.WorldToScreenPoint(wp1.transform.position)).sqrMagnitude;
		float screenDist2 = (currentScreenCoord - Camera.main.WorldToScreenPoint(wp2.transform.position)).sqrMagnitude;


		// go to waypoint 1
		if (screenDist1 < screenDist2)
		{
			Jump.DestinationWaypoint = wp1;
		}
		// go to waypoint 2
		else
		{
			Jump.DestinationWaypoint = wp2;
		}
	}


	void MoveFromWaypoint()
	{
		if (CurrentSegment != null)
		{
			CurrentState = State.Moving;

			DestinationWaypoints = new List<Waypoint>(CurrentSegment.Waypoints);

			int currentIndex = DestinationWaypoints.FindIndex(delegate(Waypoint wp) { return wp == CurrentWaypoint; });

			// we are at the end - reverse waypoints to go from 0
			if (currentIndex == DestinationWaypoints.Count - 1)
			{
				DestinationWaypoints.Reverse();
			}
		}
	}

	void WaitAtCrossRoad(Connector nearestConnectorOnSegment, List<Connector> segments)
	{
		CurrentState = State.WaitingCrossRoad;

		GameObject crossRoadGo = Instantiate(crossRoadPrefab) as GameObject;
		CrossRoad = crossRoadGo.GetComponent<CrossRoad>();

		CrossRoad.Init(nearestConnectorOnSegment);
	}

	void OnMouseDown()
	{
		if (CurrentSegment != null)
		{
			if (CrossRoad != null)
			{
				JumpToSegment(CrossRoad.CurrentConnector.Segment);

				Destroy(CrossRoad.gameObject);
			}
			else
			{
				// try jump to other segment first
				OnEndSegmentReached();

				if (CurrentState != State.Jumping)
				{

					MoveFromWaypoint();
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.collider.gameObject.GetComponent<Finish>() != null)
		//if (other.collider.gameObject.tag == "Finish")
		{
			Game.Instance.GameFinished();
		}
		else
		{
			Bonus bonus = other.collider.gameObject.GetComponent<Bonus>();
			//else if (other.collider.gameObject.tag == "Bonus")

			if (bonus != null)
			{
				Game.Instance.CollectBonus(other.collider.gameObject.GetComponent<Bonus>());
			}
		}

	}


}

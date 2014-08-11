using UnityEngine;
using System.Collections.Generic;
using System;

public class SceneController : UI.ITouchListener, UI.IObjectHitListener
{
	public enum RotDirection
	{
		X, 
		Y,
		Z
	}


	public enum Zone 
	{
		Bottom, 
		Right,
		Left,
		None
	}
	
	public Transform RootPoint { get; set; }

	public SceneController()
	{
		UI.TouchManager.Instance.Register(this);
		UI.TouchManager.Instance.RegisterObjectHitListener(this);
	}
	
	public void Release()
	{
		UI.TouchManager.Instance.Unregister(this);
		UI.TouchManager.Instance.UntegisterObjectHitListener(this);
	}

	public bool Rotating { get { return _rotateBy != null; }} 

	private Vector3 _initialRotation;
	private UI.Touch? _touchBegan;
	private Zone _touchZone;

	private Vector3? _dstRotation;
	private Vector3 _actualRotation;


	private Vector3? _rotateBy;
	private int _rotateBySteps;


	private const int ROTATION_ANGLE = 90;





	public void Update()
	{

	// NOT USED NOW
//		if (_dstRotation != null)
//		{
//
//			_actualRotation = Vector3.Lerp(_actualRotation, _dstRotation.Value, Time.deltaTime * 5.0f);
//
//
//			Vector3 newRot = _initialRotation + _actualRotation;
//			RootPoint.transform.eulerAngles = newRot;
//
//			if (RootPoint.transform.eulerAngles.x != newRot.x )
//
//
//			Debug.Log ("Rotation: " + 	RootPoint.transform.eulerAngles.ToString());
//		
//			if ( (_actualRotation - _dstRotation.Value).magnitude < 1.0f)
//			{
//				RootPoint.transform.eulerAngles = _initialRotation  + _dstRotation.Value;
//
//				_dstRotation = null;
//				Debug.Log ("EndRotation: " + 	RootPoint.transform.eulerAngles.ToString());
//
//			}
//		}

		if (_rotateBy != null)
		{
			RootPoint.transform.Rotate(_rotateBy.Value,Space.World);

			--_rotateBySteps;

			if (_rotateBySteps == 0)
			{
				_rotateBy = null;
			}
		}

	}
	
	private Zone ResolveTouchZone(Vector3 pos)
	{
		Zone zone = Zone.None;

		Camera cam = MUI.Instance.UICamera;
		Ray ray = cam.ScreenPointToRay(pos);
		
		var hits = Physics.RaycastAll(ray);
		
		if (Array.FindIndex(hits, x=>x.collider.gameObject.name == "RotationBottom") != -1)
		{
			zone = Zone.Bottom;
		}
		else if (Array.FindIndex(hits, x=>x.collider.gameObject.name == "RotationLeft") != -1)
		{
			zone = Zone.Left;
		}
		else if (Array.FindIndex(hits, x=>x.collider.gameObject.name == "RotationRight") != -1)
		{
			zone = Zone.Right;
		}
		else
		{
			zone = Zone.None;
		}
//
//		if (pos.y < Utils.ScreenSize().y * Settings.Instance.BottomReactionArea)
//		{
//			zone = Zone.Bottom;
//		}
//		else if (pos.x > Utils.ScreenSize().x * (1 - Settings.Instance.RightReactionArea))
//		{
//			zone  = Zone.Left;
//		}
//		else if (pos.x < Utils.ScreenSize().x * Settings.Instance.RightReactionArea)
//		{
//			zone = Zone.Right;
//		}
		
		return zone;
		
	}
	
	public void TouchBegan(UI.Touch touch)
	{
		if (_touchBegan == null)
		{
			_initialRotation = RootPoint.transform.localRotation.eulerAngles;
			
			_touchBegan = touch;
			
			_touchZone = ResolveTouchZone(touch.Position);

			Debug.Log ("SceneController.TouchBegan: zone: "  + _touchZone.ToString());
		}
	}
	
	
	public void TouchEnded(UI.Touch touch)
	{

		List<CrossRoadArrow> crossRoadArrows = UI.TouchManager.Instance.GetGameObjectsAt<CrossRoadArrow>(touch.Position);
		if (crossRoadArrows.Count > 0)
		{
			crossRoadArrows[0].CrossRoad.SwitchSegment();
		}
		else
		{
			List<Box> boxes = UI.TouchManager.Instance.GetGameObjectsAt<Box>(touch.Position);
			if (boxes.Count > 0)
			{
				boxes[0].ParentSegment.OnTouch();
			}
		}



		if (_touchZone != Zone.None)
		{


		}

		_rotateBy = null;
		_touchZone = Zone.None;
		_touchBegan = null;
	}
	
	public void TouchMoved(UI.Touch touch)
	{
	
		if (_touchZone != Zone.None)
		{
			Vector3 touchBeginEndVec = touch.Position - _touchBegan.Value.Position;
			
			Vector3 rotation = new Vector3();
			
			if (_touchZone == Zone.Bottom)
			{
				rotation.y = touchBeginEndVec.x * 0.4f;
			}
			else if (_touchZone == Zone.Right)
			{
				rotation.x = touchBeginEndVec.y * 0.4f;
			}
			else if (_touchZone == Zone.Left)
			{
				rotation.z = touchBeginEndVec.y * 0.4f;
			}

			rotation.x = Math.Sign(rotation.x) * Math.Min (ROTATION_ANGLE, Mathf.Abs(rotation.x));
			rotation.y = Math.Sign(rotation.y) * Math.Min (ROTATION_ANGLE, Mathf.Abs(rotation.y));
			rotation.z = Math.Sign(rotation.z) * Math.Min (ROTATION_ANGLE, Mathf.Abs(rotation.z));

			RootPoint.transform.eulerAngles = _initialRotation + rotation;
		}
	}


	public void ObjectHit(GameObject obj)
	{
		if (obj.name == "BtnArrLeft")
		{
			Rotate(SceneController.RotDirection.Y, ROTATION_ANGLE);
		}
		else if (obj.name == "BtnArrRight")
		{
			Rotate(SceneController.RotDirection.Y, -ROTATION_ANGLE);
		}
		else if (obj.name == "BtnArrUpLeft")
		{
			Rotate(SceneController.RotDirection.X, ROTATION_ANGLE);
		}
		else if (obj.name == "BtnArrDownLeft")
		{
			Rotate(SceneController.RotDirection.X, -ROTATION_ANGLE);
		}
		else if (obj.name == "BtnArrUpRight")
		{
			Rotate(SceneController.RotDirection.Z, ROTATION_ANGLE);
		}
		else if (obj.name == "BtnArrDownRight")
		{
			Rotate(SceneController.RotDirection.Z, -ROTATION_ANGLE);
		}
	}

	private void Rotate(RotDirection dir, float angle)
	{
		if (_rotateBy == null)
		{
			Game.Instance.PlayerStatus.Moves++;

			_dstRotation = new Vector3(angle, 0, 0);

			switch(dir)
			{
			case RotDirection.X:
				_dstRotation = new Vector3(angle, 0, 0);
				break;

			case RotDirection.Y:
				_dstRotation = new Vector3(0, angle, 0);
				break;

			case RotDirection.Z:
				_dstRotation = new Vector3(0, 0, angle);
				break;
			}
		
			_rotateBySteps = 10;
			_rotateBy = _dstRotation / _rotateBySteps;
		}
	}

}

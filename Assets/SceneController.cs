using UnityEngine;
using System.Collections.Generic;

public class SceneController : UI.ITouchListener, UI.IObjectHitListener
#if UNITY_EDITOR
, UI.IKeyPressedListener
#endif
{
	public enum RotDirection
	{
		Left, 
		Right,
		Up,
		Down
	}


	public enum Zone 
	{
		Bottom, 
		Right,
		None
	}
	
	public Transform RootPoint { get; set; }
	
	
	private Vector3 _initialRotation;
	private UI.Touch? _touchBegan;
	private Zone _touchZone;

	private Vector3? _dstRotation;
	private Vector3 _actualRotation;


	private Vector3? _rotateBy;
	private int _rotateBySteps;

	public SceneController()
	{
		UI.TouchManager.Instance.Register(this);
		UI.TouchManager.Instance.RegisterObjectHitListener(this);

#if UNITY_EDITOR
		UI.TouchManager.Instance.RegisterKeyListener(this);
#endif
	}
	
	public void Release()
	{
		UI.TouchManager.Instance.Unregister(this);
		UI.TouchManager.Instance.UntegisterObjectHitListener(this);

	
#if UNITY_EDITOR
		UI.TouchManager.Instance.UnregisterKeyListener(this);
#endif
	}



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
		
		if (pos.y < Utils.ScreenSize().y * Settings.Instance.BottomReactionArea)
		{
			zone = Zone.Bottom;
		}
		else if (pos.x > Utils.ScreenSize().x * (1 - Settings.Instance.RightReactionArea))
		{
			zone  = Zone.Right;
		}
		
		return zone;
		
	}
	
	public void TouchBegan(UI.Touch touch)
	{
		if (_touchBegan == null)
		{
			_initialRotation = RootPoint.transform.localRotation.eulerAngles;
			
			_touchBegan = touch;
			
			_touchZone = ResolveTouchZone(touch.Position);
		}
	}
	
	
	public void TouchEnded(UI.Touch touch)
	{
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
				rotation.y = touchBeginEndVec.x * 0.2f;
			}
			else if (_touchZone == Zone.Right)
			{
				rotation.x = touchBeginEndVec.y * 0.2f;
			}
			
			RootPoint.transform.eulerAngles = _initialRotation + rotation;
		}
	}


	public void ObjectHit(GameObject obj)
	{
		if (obj.name == "BtnArrLeft")
		{
			Rotate(SceneController.RotDirection.Left);
		}
		else if (obj.name == "BtnArrRight")
		{
			Rotate(SceneController.RotDirection.Right);
		}
		else if (obj.name == "BtnArrUp")
		{
			Rotate(SceneController.RotDirection.Up);
		}
		else if (obj.name == "BtnArrDown")
		{
			Rotate(SceneController.RotDirection.Down);
		}
	}

	public void Rotate(RotDirection dir)
	{
		if (_rotateBy == null)
		{
			switch(dir)
			{
			case RotDirection.Down:
				_dstRotation = new Vector3(-90,0,0);
				break;

			case RotDirection.Up:
				_dstRotation = new Vector3(90,0,0);
				break;

			case RotDirection.Left:
				_dstRotation = new Vector3(0,90,0);
				break;

			case RotDirection.Right:
				_dstRotation = new Vector3(0,-90,0);
				break;

			}
		
			_rotateBySteps = 10;
			_rotateBy = _dstRotation / _rotateBySteps;
		}
	}

#if UNITY_EDITOR
	public void KeyPressed(KeyCode keyCode)
	{
		// ignore of rotation in progress
		if (_dstRotation != null && _rotateBy != null)
			return;


		if (keyCode == KeyCode.LeftArrow)
		{
			_dstRotation = new Vector3(0,90,0);
		}
		else if (keyCode == KeyCode.RightArrow)
		{
			_dstRotation = new Vector3(0,-90,0);
		}
		else if (keyCode == KeyCode.UpArrow)
		{
			_dstRotation = new Vector3(90,0,0);
		}
		else if (keyCode == KeyCode.DownArrow)
		{
			_dstRotation = new Vector3(-90,0,0);
		}

		_actualRotation = Vector3.zero;
		_initialRotation = RootPoint.transform.eulerAngles;


		_rotateBySteps = 10;
		_rotateBy = _dstRotation / _rotateBySteps;
	}
#endif
	
}

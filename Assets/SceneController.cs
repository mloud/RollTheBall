using UnityEngine;
using System.Collections.Generic;

public class SceneController : UI.ITouchListener
#if UNITY_EDITOR
, UI.IKeyPressedListener
#endif
{
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

	public SceneController()
	{
		UI.TouchManager.Instance.Register(this);

#if UNITY_EDITOR
		UI.TouchManager.Instance.RegisterKeyListener(this);
#endif
	}
	
	public void Release()
	{
		UI.TouchManager.Instance.Unregister(this);

	
#if UNITY_EDITOR
		UI.TouchManager.Instance.UnregisterKeyListener(this);
#endif
	}



	public void Update()
	{
		if (_dstRotation != null)
		{
			_actualRotation = Vector3.Lerp(_actualRotation, _dstRotation.Value, Time.deltaTime * 5.0f);

			RootPoint.transform.eulerAngles = _initialRotation + _actualRotation;

			Debug.Log ("Rotation: " + 	RootPoint.transform.eulerAngles.ToString());
		
			if ( (_actualRotation - _dstRotation.Value).magnitude < 1.0f)
			{
				RootPoint.transform.eulerAngles = _initialRotation  + _dstRotation.Value;
				_dstRotation = null;
				Debug.Log ("EndRotation: " + 	RootPoint.transform.eulerAngles.ToString());

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


#if UNITY_EDITOR
	public void KeyPressed(KeyCode keyCode)
	{
		// ignore of rotation in progress
		if (_dstRotation != null)
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

		Debug.Log ("Destination rotation: " + (_initialRotation + _dstRotation).ToString() );

	}
#endif
	
}

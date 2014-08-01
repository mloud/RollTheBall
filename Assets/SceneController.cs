using UnityEngine;
using System.Collections.Generic;

public class SceneController : UI.ITouchListener
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


	public SceneController()
	{
		UI.TouchManager.Instance.Register(this);
	}

	public void Release()
	{
		UI.TouchManager.Instance.Unregister(this);
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

}

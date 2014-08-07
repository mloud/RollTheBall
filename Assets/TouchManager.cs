using UnityEngine;
using System.Collections.Generic;


namespace UI
{
	public struct Touch
	{
		public Touch(int id, Vector3 pos)
		{
			Id = id;
			Position = pos;
		}

		public int Id;
		public Vector3 Position;
	}

	public interface ITouchListener
	{
		void TouchBegan(Touch touch);
		void TouchEnded(Touch touch);
		void TouchMoved(Touch touch);
	}

	public interface IKeyPressedListener
	{
		void KeyPressed(KeyCode keyCode);
	}

	public interface IObjectHitListener
	{
		void ObjectHit(GameObject obj);
	}


	public class TouchManager : MonoBehaviour
	{
		public static TouchManager Instance { get { return _instance; }}

		private static TouchManager _instance;

		private List<ITouchListener> TouchListeners { get; set; }

		private List<IObjectHitListener> ObjectHitListeners{ get; set; }

#if UNITY_EDITOR
		private List<IKeyPressedListener> KeyPressListeners { get; set; }
#endif

		private List<Camera> Cameras { get; set; }

		void Awake()
		{
			_instance = this;
			TouchListeners = new List<ITouchListener>();

			ObjectHitListeners = new List<IObjectHitListener>();

#if UNITY_EDITOR
			KeyPressListeners = new List<IKeyPressedListener>();
#endif

			Cameras = new List<Camera>(GameObject.FindObjectsOfType<Camera>());
		}


		public void Register(ITouchListener listener)
		{
			TouchListeners.Add(listener);
		}

		public void Unregister(ITouchListener listener)
		{
			TouchListeners.Remove(listener);
		}

		public void RegisterObjectHitListener(IObjectHitListener listener)
		{
			ObjectHitListeners.Add(listener);
		}

		public void UntegisterObjectHitListener(IObjectHitListener listener)
		{
			ObjectHitListeners.Remove(listener);
		}

#if UNITY_EDITOR

		public void RegisterKeyListener(IKeyPressedListener listener)
		{
			KeyPressListeners.Add(listener);
		}

		public void UnregisterKeyListener(IKeyPressedListener listener)
		{
			KeyPressListeners.Remove(listener);
		}
#endif



		void Update ()
		{
			ProcessTouch();

#if UNITY_EDITOR
			ProcessKeyPress();	
#endif
		}


#if UNITY_EDITOR

		private void KeyPress(KeyCode keyCode)
		{
			for (int i = 0; i < KeyPressListeners.Count; ++i)
			{
				KeyPressListeners[i].KeyPressed(keyCode);
			}
		}

#endif
	
		private void TouchBegan(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchBegan(touch);
			}
		}

		private void TouchMoved(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchMoved(touch);
			}
		}

		private void TouchEnded(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchEnded(touch);
			}

			ProcessHitButtons(touch.Position);
		}

		public List<T> GetGameObjectsAt<T>(Vector3 pos) where T: MonoBehaviour
		{
			List<T> result = new List<T>();


			for (int i = 0; i < Cameras.Count; ++i)
			{
				RaycastHit[] hits;

				Camera cam = Cameras[i];
				Ray ray = cam.ScreenPointToRay(pos);

				hits = Physics.RaycastAll(ray);

				for (int j = 0; j < hits.Length; ++j)
				{
					T comp = hits[j].collider.gameObject.GetComponent<T>();

					if (comp != null)
					{
						result.Add(comp);
					}
				}
			}

			return result;
		}


		private void ProcessHitButtons(Vector3 touchPos)
		{
			List<Button> buttons = GetGameObjectsAt<Button>(touchPos);

			if (buttons.Count > 0)
			{
				for (int i = 0; i < ObjectHitListeners.Count; ++i)
				{
					ObjectHitListeners[i].ObjectHit(buttons[0].gameObject);
				}
			}
		}


#if UNITY_EDITOR
		private bool mouseMoving;
		void ProcessTouch()
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseMoving = true;
				TouchBegan(new Touch(0, Input.mousePosition));
			}
			else if (Input.GetMouseButtonUp(0))
			{
				mouseMoving = false;
				TouchEnded(new Touch(0, Input.mousePosition));
			}

			if (mouseMoving)
			{
				TouchMoved(new Touch(0, Input.mousePosition));
			}
		}

		void ProcessKeyPress()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				KeyPress(KeyCode.LeftArrow);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				KeyPress(KeyCode.RightArrow);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				KeyPress(KeyCode.DownArrow);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				KeyPress(KeyCode.UpArrow);
			}
		}

#else
		void ProcessTouch()
		{

			for (int i = 0; i < Input.touchCount; ++i)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					TouchBegan(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
				else if (Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					TouchEnded(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
				else if (Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					TouchMoved(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
			}
		}
#endif


	}
}
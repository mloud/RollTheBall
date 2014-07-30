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

	public class TouchManager : MonoBehaviour
	{
		public static TouchManager Instance { get { return _instance; }}

		private static TouchManager _instance;

		private List<ITouchListener> TouchListeners { get; set; }


		void Awake()
		{
			_instance = this;
			TouchListeners = new List<ITouchListener>();
		}


		public void Register(ITouchListener listener)
		{
			TouchListeners.Add(listener);
		}

		public void Unregister(ITouchListener listener)
		{
			TouchListeners.Remove(listener);
		}


		void Update ()
		{
			ProcessTouch();
		}

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
		}

		public List<T> GetGameObjectsAt<T>(Vector3 pos) where T: MonoBehaviour
		{
			List<T> result = new List<T>();

			RaycastHit[] hits;

			Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			Ray ray = cam.ScreenPointToRay(pos);

			hits = Physics.RaycastAll(ray);

			for (int i = 0; i < hits.Length; ++i)
			{
				T comp = hits[i].collider.gameObject.GetComponent<T>();

				if (comp != null)
				{
					result.Add(comp);
				}
			}

			return result;
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
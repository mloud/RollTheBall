using UnityEngine;
using UnityEditor;
 
public class SceneViewCameraTest : ScriptableObject
{
    [MenuItem("MultiCamera/SetEditorCameras")]
    static public void SetCameras()
    {

		// Get all Cameras 
		Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
	
		// Find GameCamera
		Camera gameCamera = ArrayUtility.Find(cameras, x=>x.tag == "MainCamera");


		GameObject editorGameObject = GameObject.Find("Editor");

		Transform root = GameObject.Find("Root").transform;

		Debug.Log ("GameCamera");
		Debug.Log (" Position:" + gameCamera.transform.position.ToString());
		Debug.Log (" Rotation:" + gameCamera.transform.eulerAngles.ToString());
		Debug.Log (" Ortho:" + gameCamera.isOrthoGraphic.ToString());


		for (int i = 0; i < SceneView.sceneViews.Count; ++i)
		{
			SceneView sw = SceneView.sceneViews[i] as SceneView;

			if (sw != null)
			{
			
				sw.name = "Scene View " + i.ToString();
				sw.title = "Scene View " + i.ToString();

				GameObject target = new GameObject();

				target.transform.parent = editorGameObject.transform;


				target.transform.position = gameCamera.transform.position;
				target.transform.rotation = gameCamera.transform.rotation;
				target.transform.RotateAround(root.position, Vector3.up, -i * 90.0f);

				sw.AlignViewToObject(target.transform);
			
				sw.camera.orthographicSize = gameCamera.orthographicSize;
				sw.camera.aspect = gameCamera.aspect;


				bool destroyDummyGo = true;

				// create special Scene view Camera
				if (editorGameObject != null)
				{
					//Find Scene View Cameras
					Camera sceneViewCamera = ArrayUtility.Find(cameras, x=>x.name == ("SceneCamera_" + i.ToString()));
			
					// doesn't exist -> create
					if (sceneViewCamera == null)
					{
						destroyDummyGo = false; // keep game object and attach SceneViewCamera
						target.name = "SceneCamera_" + i;

						sceneViewCamera = target.AddComponent<Camera>();
						CopyCamareSettings(gameCamera, sceneViewCamera);
					}
				}
			

				if (destroyDummyGo)
				{
					GameObject.DestroyImmediate(target);
				}

				Debug.Log ("Camera set in SceneView " + i.ToString());
			}
		
		}
	}

	private static void CopyCamareSettings(Camera camFrom, Camera camTo)
	{
		camTo.isOrthoGraphic = camFrom.isOrthoGraphic;
		camTo.nearClipPlane = camFrom.nearClipPlane;
		camTo.orthographicSize = camFrom.orthographicSize;
		camTo.aspect = camFrom.aspect;
		camTo.depth = camFrom.depth;
	}


	[MenuItem("MultiCamera/ResetRotationToDefault")]
	static public void resetToDefault()
	{
		Transform root = GameObject.Find("Root").transform;
		root.localEulerAngles = new Vector3 (135, 45, 0);
	}

	[MenuItem("MultiCamera/RotateRight")]
	static public void rotateRight()
	{
		Transform root = GameObject.Find("Root").transform;
		root.Rotate(new Vector3(0, 90, 0), Space.World);
	}

	[MenuItem("MultiCamera/RotateLeft")]
	static public void rotateLeft()
	{
		Transform root = GameObject.Find("Root").transform;
		root.Rotate(new Vector3(0, -90, 0), Space.World);
	}

	[MenuItem("MultiCamera/RotateUp")]
	static public void rotateUp()
	{
		Transform root = GameObject.Find("Root").transform;
		root.Rotate(new Vector3(90, 0, 0), Space.World);
	}
	
	[MenuItem("MultiCamera/RotateDown")]
	static public void rotateDown()
	{
		Transform root = GameObject.Find("Root").transform;
		root.Rotate(new Vector3(-90, 0, 0), Space.World);
	}





}
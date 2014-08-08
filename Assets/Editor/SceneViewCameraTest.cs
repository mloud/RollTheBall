using UnityEngine;
using UnityEditor;
 
public class SceneViewCameraTest : ScriptableObject
{
	[MenuItem("MultiCamera/SetCamerasForXRotation ")]
	static public void SetCamerasForXRotation()
	{
		SetCameras(new Vector3(1,0,0));
	}

	[MenuItem("MultiCamera/SetCamerasForYRotation ")]
	static public void SetCamerasForYRotation()
	{
		SetCameras(new Vector3(0,1,0));
	}

	[MenuItem("MultiCamera/SetCamerasForZRotation ")]
	static public void SetCamerasForZRotation()
	{
		SetCameras(new Vector3(0,0,1));
	}




    static private void SetCameras(Vector3 rotationVector)
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

			if (sw != null && sw.camera != null)
			{
			
				sw.name = "Scene View " + i.ToString();
				sw.title = "Scene View " + i.ToString();

				GameObject target = new GameObject();

				target.transform.parent = editorGameObject.transform;


				target.transform.position = gameCamera.transform.position;
				target.transform.rotation = gameCamera.transform.rotation;
				target.transform.RotateAround(root.position, rotationVector, -i * 90.0f);

				sw.AlignViewToObject(target.transform);

				CopyCamareSettings(gameCamera, sw.camera);

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
					}
					CopyCamareSettings(gameCamera, sceneViewCamera);
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
		camTo.farClipPlane = camFrom.farClipPlane;
		camTo.orthographicSize = camFrom.orthographicSize;
		camTo.aspect = camFrom.aspect;
		camTo.depth = camFrom.depth;
	}



}
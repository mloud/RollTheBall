using UnityEngine;
using UnityEditor;
 
public class SceneViewCameraTest : ScriptableObject
{
    [MenuItem("MultiCamera/SetToGameCamera")]
    static public void MoveSceneViewCamera()
    {
		// Get GameCamera 
		Camera gameCamera = (Camera)Object.FindObjectOfType(typeof(Camera));
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
				target.transform.position = gameCamera.transform.position;
				target.transform.rotation = gameCamera.transform.rotation;

				target.transform.RotateAround(root.position, Vector3.up, -i * 90.0f);


				sw.AlignViewToObject(target.transform);
				GameObject.DestroyImmediate(target);
	
			

				Debug.Log ("Camera set in SceneView " + i.ToString());
			}
		
		}
	  }
}
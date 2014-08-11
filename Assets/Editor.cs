using UnityEngine;
using System.Collections;
using UnityEditor;

public class Editor : MonoBehaviour
{

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{

			Segment[] segments = GameObject.FindObjectsOfType<Segment>();

			Gizmos.color = Color.green;
			for (int i = 0; i < segments.Length; ++i)
			{
				for (int j = i + 1; j < segments.Length; ++j)
				{

					for (int k = 0; k < SceneView.sceneViews.Count; ++k)
					{
						SceneView sw = SceneView.sceneViews[k] as SceneView;

						if (sw != null)
						{
							Utils.SegmentConnection segConn = Utils.IsSegmentConnected(sw.camera, segments[i], segments[j]);

							if (segConn != null)
							{
								Gizmos.DrawSphere(segConn.Conn1.transform.position, 0.3f);
								Gizmos.DrawSphere(segConn.Conn2.transform.position, 0.3f);
							}
						}
						
					}
				}
			}
		}
	}
	#endif
}

using UnityEngine;
using System.Collections;

public class CrossRoadArrow : MonoBehaviour 
{
	[SerializeField]
	Renderer arrowRenderer;

	[SerializeField]
	Color disabledColor;


	public bool Enabled { get; set; }

	public CrossRoad CrossRoad { get; set; }

	public void Enable()
	{
		arrowRenderer.material.color = Color.white;

		Enabled = true;
	}

	public void Disable()
	{
		arrowRenderer.material.color = disabledColor;

		Enabled = false;
	}


	void Update ()
	{
	
	}
}

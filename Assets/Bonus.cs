using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour 
{
	[SerializeField]
	float rotationSpeed;

	[SerializeField]
	SpriteRenderer sprite;

	Vector3? DstPosition;
	Vector3? SrcPosition;
	float Distance { get; set; }

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateRotation();

		if (DstPosition != null)
		{
			transform.position = Vector3.Lerp(transform.position, DstPosition.Value, Time.deltaTime * 10.0f);

			Color color = sprite.material.color;
			color.a =  1 - (transform.position - DstPosition.Value).magnitude / Distance;

			sprite.renderer.material.color = color;


			if ( (transform.position - DstPosition.Value).sqrMagnitude < 0.1f * 0.1f)
			{
				Destroy(gameObject);
			}
		}
	}

	void UpdateRotation()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
	}

	public void FlyUp()
	{
		SrcPosition = transform.position;
		DstPosition = SrcPosition.Value + Vector3.up * 3.0f;
		Distance = (DstPosition.Value - SrcPosition.Value).magnitude;
	}

}

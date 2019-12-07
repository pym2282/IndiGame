using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public float speed = 3f;
	[SerializeField] private float originalPosition;
	[SerializeField] private float returnPosition = -70f;

	private void Start()
	{
		originalPosition = transform.position.y;
	}

	private void FixedUpdate()
    {
		transform.position -= new Vector3(0f, speed);

		if (transform.position.y < returnPosition) {
			transform.position = new Vector3(transform.position.x, originalPosition);
		}
	}
}

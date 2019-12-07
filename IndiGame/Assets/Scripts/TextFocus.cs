using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFocus : MonoBehaviour
{
	[SerializeField] private float textFoucsSpeed = 1f;
	[SerializeField] private float waitTime = 0.033f;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine("textFocusMax");
	}

	IEnumerator textFocusMax() {
		while (transform.localScale.x <= 1.2f)
		{
			gameObject.transform.localScale = new Vector3(transform.localScale.x + (textFoucsSpeed * Time.deltaTime), transform.localScale.y + (textFoucsSpeed * Time.deltaTime), transform.localScale.z);

			yield return new WaitForSeconds(waitTime);
		}

		StartCoroutine("textFocusMin");
	}

	IEnumerator textFocusMin() {
		while (transform.localScale.x >= 1.0f)
		{
			gameObject.transform.localScale = new Vector3(transform.localScale.x - (textFoucsSpeed * Time.deltaTime), transform.localScale.y - (textFoucsSpeed * Time.deltaTime), transform.localScale.z);

			yield return new WaitForSeconds(waitTime);
		}

		StartCoroutine("textFocusMax");
	}
}

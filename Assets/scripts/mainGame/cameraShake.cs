using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour {
	

	public IEnumerator Shake (float duration,float magnitude) {
		Vector3 originalPosition = transform.localPosition;

		float elapsedSecond = 0.0f;

		while (elapsedSecond < duration) {
			float x = Random.Range(-1.0f, 1.0f) * magnitude;
			float y = Random.Range(-1.0f, 1.0f) * magnitude;

			transform.localPosition = new Vector3(x, y);
			elapsedSecond += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = originalPosition;
	}
}

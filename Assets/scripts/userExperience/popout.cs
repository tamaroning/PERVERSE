using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popout : MonoBehaviour {

	public AnimationCurve popCurve;

	public IEnumerator popoutAnimate(float duration, GameObject obj) {
		float elapsedTime = 0f;
		obj.SetActive(true);
		float width = obj.transform.localScale.x;
		float height = obj.transform.localScale.y;
		obj.transform.localScale = Vector3.zero;

		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			float per = elapsedTime / duration;
			float curvePer = popCurve.Evaluate(per);
			obj.transform.localScale = new Vector3(width * curvePer, height * curvePer);
			yield return null;
		}
		obj.transform.localScale = new Vector3(width, height);

	}

	public IEnumerator popfadeAnimate(float duration, GameObject obj) {
		float elapsedTime = 0f;
		float width = obj.transform.localScale.x;
		float height = obj.transform.localScale.y;

		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			float per =1f- elapsedTime / duration;
			float curvePer = popCurve.Evaluate(per);
			obj.transform.localScale = new Vector3(width * curvePer, height * curvePer);
			yield return null;
		}
		obj.transform.localScale = new Vector3(width, height);
		obj.SetActive(false);
	}

	public IEnumerator rankpopoutAnimate(float duration, GameObject obj,float width,float height) {
		float elapsedTime = 0f;
		obj.SetActive(true);
		obj.transform.localScale = Vector3.zero;

		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			float per = elapsedTime / duration;
			float curvePer = popCurve.Evaluate(per);
			obj.transform.localScale = new Vector3(width * curvePer, height * curvePer);
			yield return null;
		}
		obj.transform.localScale = new Vector3(width, height);

	}

	public IEnumerator rankpopfadeAnimate(float duration, GameObject obj, float width, float height) {
		float elapsedTime = 0f;

		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			float per = 1f - elapsedTime / duration;
			float curvePer = popCurve.Evaluate(per);
			obj.transform.localScale = new Vector3(width * curvePer, height * curvePer);
			yield return null;
		}
		obj.transform.localScale = new Vector3(width, height);
		obj.SetActive(false);
	}
}

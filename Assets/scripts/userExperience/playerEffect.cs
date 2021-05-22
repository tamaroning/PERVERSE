using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class playerEffect : MonoBehaviour {
	public float duration,maxHeight,minHeight;
	static float  height, area;
	public float startTime;
	public AnimationCurve curve;
	public Image img;
	// Use this for initialization
	void Start() {
		height = transform.localScale.y;
		startTime = Time.time;
	}
	int cnt = 0;
	// Update is called once per frame
	void LateUpdate() {
		float rate = 1.0f - (Time.time - startTime) / duration;
		if (rate <= 0) {
			cnt = 0;
			img.enabled = false;
			gameObject.SetActive(false);
		}
		float per = curve.Evaluate(rate);
		height = Mathf.Lerp(minHeight, maxHeight, per);

		if (Vector3.Distance(transform.parent.GetComponent<mainPlayerAnimation>().toPos, transform.parent.transform.position) > 10f) {
			cnt++;
			if (cnt > 2) {
				img.enabled = true;
				transform.localScale = new Vector3(height, height, 1);
				img.color = new Color(img.color.r, img.color.g, img.color.b,rate);
			}

		}
		else {
			cnt = 0;
			img.enabled = false;

			gameObject.SetActive(false);
		}
	}
}

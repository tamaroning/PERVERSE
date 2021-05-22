using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleColorChange : MonoBehaviour {

	public Camera cam;
	Camera camRender;
	float h, s, v;
	public float timeDuration;
	float start;
	
	// Update is called once per frame
	void Start() {
		camRender = GetComponent<Camera>();
		s = 0.37f;
		v = 0.39f;
		start = Time.time;
	}
	void Update () {
		if (Time.time - start > timeDuration) {
			start += timeDuration;
		}
		h = (Time.time - start) / timeDuration;
		camRender.backgroundColor = Color.HSVToRGB(h, s, v);

	}
}

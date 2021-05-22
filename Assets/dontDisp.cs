using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDisp : MonoBehaviour {
	public GameObject bulb, h1,h2,h3,h4,rest;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bulb.activeSelf && (h1.activeSelf|| h2.activeSelf || h3.activeSelf || h4.activeSelf||rest.activeSelf)) {
			bulb.SetActive(false);
		}
	}
}

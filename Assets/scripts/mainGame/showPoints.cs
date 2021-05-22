using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showPoints : MonoBehaviour {

    public static int points=0;

	public void goal() {
		points++;
	}
	public void reset() {
		points = 0;
	}
	public int retPoint() {
		return points;
	}
	// Use this for initialization
	void Start () {
        this.GetComponentInChildren<Text>().text = points+"";
	}
	
	// Update is called once per frame
	void Update () {

	}
}

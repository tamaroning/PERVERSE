using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showTimesToGoal : MonoBehaviour {

    private int times=0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponentInChildren<Text>().text = times + " times\nto goal";
	}
}

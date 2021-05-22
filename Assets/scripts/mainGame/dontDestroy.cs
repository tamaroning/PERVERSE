using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class dontDestroy : MonoBehaviour {

	private void Awake() {
		DontDestroyOnLoad(gameObject);
		if (SceneManager.GetActiveScene().name == "music") {
			SceneManager.LoadScene("Title");
		}
		try {
			Destroy(GameObject.Find("dontDestroy (1)"));

		}
		catch {
			GameObject.Find("timeText").GetComponent<timeCounter>().reset();
			GameObject.Find("pointsText").GetComponent<showPoints>().reset();

		}
	}
}

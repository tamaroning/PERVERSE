using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class playBattleUserButtons : MonoBehaviour {

    public AudioSource AS;
    public AudioClip open;
    public AudioClip close;

	bool isFirst = false;
	// Use this for initialization
	void Start () {
		isFirst = (PlayerPrefs.GetInt("uid",-1)==-1);
	}
	
	public void userButton() {
        if (PlayerPrefs.GetInt("uid", -1) != -1) {
            Debug.Log("your user data was not found,plase register first of all");
			return;
		}

	}
	public GameObject outFade;
	IEnumerator loadScene(string name) {
		outFade.SetActive(true);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}

	public void rankingLoad() {
        if (getMap.isLoading) return;
		if (PlayerPrefs.GetInt("uid",-1)!=-1) {
			StartCoroutine(loadScene("Ranking"));
        }else{
            Debug.Log("your user data was not found,plase register first of all");
        }
	}
	public void tutorialLoad() {
		StartCoroutine(loadScene("Tutorial"));
	}
	public void playButton() {
        if (getMap.isLoading) return;

        if (PlayerPrefs.GetInt("uid", -1) == -1) {
			StartCoroutine(loadScene("Tutorial"));
		}

		else {
			StartCoroutine(loadScene("Debug"));
		}
	}
}

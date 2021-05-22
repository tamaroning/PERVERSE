using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class userNameSet : MonoBehaviour {
	public GameObject playerNamePanel;
    public transceive Transceive;

    public AudioClip decision;

    int retUid = -1;

	private void Start() {
        if (PlayerPrefs.GetInt("uid",-1)==-1) {
			StartCoroutine(GetComponent<popout>().popoutAnimate(1.5f, playerNamePanel));
		}
		GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().pauseTrigger = true;
	}


	public void okButtonOnClick() {
		//ユーザーネーム
		string userName = GameObject.Find("userNameText").GetComponent<Text>().text;
		Debug.Log(userName);

        if (userName.Length==0 || 15<userName.Length) {
            Debug.Log("Username's length　is Inappropriate, try again");
            return;
        }//ここにユーザーネームの条件分を書いて


        GameObject ASobj = GameObject.Find("EventSystem").gameObject;
        AudioSource AS = ASobj.GetComponent<AudioSource>();
        AS.clip = decision;
        AS.volume = 0.4F;
        AS.Play();


        StartCoroutine(Transceive.userRegistration(userName, (r) => retUid = r));

		StartCoroutine(GetComponent<popout>().popfadeAnimate(.7f, playerNamePanel));
		GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().pauseTrigger=false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timeCounter : MonoBehaviour {

	private int timeLimit = 30;
	public int showTime;
	public static float elapsedSeconds = 0.0f;//経過秒
	bool alreadyFadeout = false;
	Image scoreBig, scoreBig2;
	public Color almostEndColor;
	Color startColor;
	public TypefaceAnimator typeScript;
	static bool plusFlag = false;
	static int nowT = 0, minT = 0;
	public bool sceneIsLoaded = false;
	
	public AnimationCurve curveFill;
	int difficulty = 0;
	public Text firstTimeTxt;
	public bool isGameOver = false;
	public Text timeLeftSmall;
	public GameObject inHand, outHand;
	public Sprite[] hands;
	public Animator inHandAnimator;
	static int handImageNum;
	public AudioClip timeoutAudio;
	public void handImageNumSet(int a) {
		handImageNum = a;
		if (GetComponent<showPoints>().retPoint() % 5 == 4) {
			outHand.SetActive(true);
			outHand.GetComponent<Image>().sprite = hands[a];
		}
	}

	IEnumerator startFillTimeGage(float duration, float fillSize) {
		float animElapsedTime = 0.0f;
		float startSize = scoreBig.fillAmount;
		float startSize2 = scoreBig2.fillAmount;
		enabled = false;
		while (animElapsedTime < duration) {
			animElapsedTime += Time.deltaTime;
			float per = animElapsedTime / duration;
			scoreBig.fillAmount = Mathf.Min(1.0f, Mathf.Lerp(startSize, startSize + fillSize / timeLimit, curveFill.Evaluate(per)));
			if (startSize2 > 0) {
				scoreBig2.fillAmount = Mathf.Max(0.0f, Mathf.Lerp(startSize - 1.0f, startSize2 + fillSize / timeLimit, curveFill.Evaluate(per)));
			}
			else {
				scoreBig2.fillAmount = Mathf.Max(0.0f, Mathf.Lerp(startSize2, startSize - 1f + fillSize / timeLimit, curveFill.Evaluate(per)));
			}

			yield return null;
		}
		enabled = true;
	}

	public float timeCntFirst = 0f;

	public void reset() {
		elapsedSeconds = 0.0f;
		plusFlag = false;
	}


	void Awake() {
		sceneIsLoaded = true;
		scoreBig = GameObject.Find("scoreWallSmall").GetComponent<Image>();
		scoreBig2 = GameObject.Find("scoreWallSmall2").GetComponent<Image>();
		startColor = scoreBig.color;

		if (GetComponent<showPoints>().retPoint() == 0) {
			scoreBig.fillAmount = 0f;
			scoreBig2.fillAmount = 0f;
			StartCoroutine(startFillTimeGage(1f, timeLimit));
		}
		else {
			if (GetComponent<showPoints>().retPoint() % 5 == 0) {
				inHand.SetActive(true);
				inHand.GetComponent<Image>().sprite = hands[handImageNum];
			}
			timeCntFirst = 5f;
		}
		difficulty = GetComponent<data>().difficulty;

	}

	public void timeCount(int nowTime, int minTime) {
		nowT = nowTime;
		plusFlag = true;
		minT = minTime;
		Debug.Log(minT + " " + nowT);
	}

	public Color good, middle, bad;

	void timeCalc(float a, float b, float c) {
		GameObject timePlus = GameObject.Find("timePlus");
		if (nowT - minT == 0) {
			timePlus.GetComponent<Text>().text = "+" + a + "s";
			timePlus.GetComponent<Text>().color = good;
			typeScript.enabled = true;
			typeScript.colorTo = good;

			elapsedSeconds -= a;
			StartCoroutine(startFillTimeGage(0.2f, a));
		}
		else if (nowT - minT <= 5) {
			timePlus.GetComponent<Text>().text = "+" + b + "s";
			typeScript.enabled = true;
			typeScript.colorTo = middle;

			elapsedSeconds -= b;
			StartCoroutine(startFillTimeGage(0.2f, b));
		}
		else {
			timePlus.GetComponent<Text>().text = "+" + c + "s";
			typeScript.enabled = true;
			typeScript.colorTo = bad;
			elapsedSeconds -= c;
			StartCoroutine(startFillTimeGage(0.2f, c));
		}
		plusFlag = false;
	}
	// Update is called once per frame
	bool firstTrigger = false;
	bool timeLeftTrigger = false;
	void Update() {
		timeCntFirst += Time.deltaTime;
		if (timeCntFirst <= 3f && SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "mapcode") {
			firstTimeTxt.text = "" + (3 - (int)timeCntFirst);
		}
		else if (firstTrigger == false) {
			firstTrigger = true;
			if (SceneManager.GetActiveScene().name != "Tutorial") {
				firstTimeTxt.text = "";
			}
			StartCoroutine(GameObject.Find("goalAnimInMask").GetComponent<goalMaskAnim>().animStart(0.7f, 2f, 0.3f));
			StartCoroutine(GameObject.Find("goalAnimInMask2").GetComponent<goalMaskAnim>().animStart(0.7f, 2f, 0.3f));
			if (SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "mapcode") {
				inHandAnimator.enabled = true;
			}
		}
		if (GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().pauseTrigger == false && timeCntFirst > 3f) {
			elapsedSeconds += Time.deltaTime;
			if (SceneManager.GetActiveScene().name == "mapcode") {
				elapsedSeconds = 0f;
			}
		}
		showTime = timeLimit - (int)Mathf.Floor(elapsedSeconds);
		float per = Mathf.Min(1.0f, 1.0f - (elapsedSeconds / timeLimit));
		scoreBig.fillAmount = per;
		scoreBig2.fillAmount = Mathf.Max(0.0f, -((elapsedSeconds) / timeLimit));
		scoreBig.color = Color.Lerp(almostEndColor, startColor, per);

		if (showTime < 10&&showTime>=0) {
			timeLeftSmall.text = showTime + "";
			timeLeftTrigger = true;
		}
		else if (timeLeftTrigger) {
			timeLeftSmall.text = "";
		}

		if (showTime <= 0&&GameObject.Find("playerObjectUp").GetComponent<movePlayer>().goalTrigger==false) {
			if (!alreadyFadeout) {
				GetComponent<scoreFadeout>().timeOut();
				alreadyFadeout = true;
				AudioSource AS = GetComponent<AudioSource>();
				AS.clip = timeoutAudio;
				AS.volume = 0.8F;
				AS.pitch = 2F;
				AS.Play();
			}
			showTime = 0;
			isGameOver = true;
		}
		//this.GetComponentInChildren<Text>().text = showTime+"";
		if (plusFlag && sceneIsLoaded) {
			if (difficulty == 0) {
				timeCalc(5f, 3f, 1f);
			}
			else if (difficulty == 1) {
				timeCalc(7f, 5f, 3f);
			}
			else {
				timeCalc(10f, 7f, 5f);
			}
		}
		sceneIsLoaded = false;
	}


	public float getElapsedSeconds() {
		return elapsedSeconds;
	}


}
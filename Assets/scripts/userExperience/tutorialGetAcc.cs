//端末の加速度から 
//あとで実機テストします 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialGetAcc : MonoBehaviour {


	public touch touchScript;
	public int lastTouch;
	public float durationToHint;
	float lastTouchTime = 0.0f;
	float elapsedTime = 0.0f;
	private Vector3 acc;//端末の加速度 
	private GUIStyle labelStyle;
	//関数の返り値 
	public int ret = 404;
	public mainPlayerAnimation animationScriptUp, animationScriptDown;
	public movePlayer playerCalc;
	public int wantingDir=9;
	public int getDirection() {
		if (GameObject.Find("pointsText").GetComponent<data>().isTutorialMode) {
			if (tutorialSystem.wantAcc != ret) {
				ret = 0;
			}
			else {
				//return tutorialSystem.wantAcc;
			}
		}
		return ret;
	}
	public int beforeDirection = 404;
	public int handCount = 0;
	public GameObject leftHint, upHint, downHint, rightHint, moveAnimUp, moveAnimDown;
	public Image moveAnimUpImg, moveAnimDownImg;
	public Color good, middle, bad, nowColor;
	cameraShake cameraShake;

	void ifInput(int ret) {
		animationScriptUp.startTime = Time.time;
		animationScriptDown.startTime = Time.time;
		animationScriptUp.direction = ret;
		animationScriptDown.direction = ret;
		playerCalc.moveDirection = ret;
		//animationScriptUp.toPos = transform.position + new Vector3(0, -4, 0);
		moveAnimDown.SetActive(true);
		moveAnimUp.SetActive(true);
		moveAnimUp.GetComponent<playerEffect>().startTime = Time.time;
		moveAnimDown.GetComponent<playerEffect>().startTime = Time.time;
		if (ret == wantingDir || wantingDir == -1) {
			animationScriptUp.moveTrigger = true;
			animationScriptDown.moveTrigger = true;
			playerCalc.moveTrigger = true;
			if (wantingDir != -1) {
				GameObject.Find("EventSystem").GetComponent<tutorialSystem>().isOkClicked = true;
			}
		}

		//StartCoroutine(cameraShake.Shake(.15f, .8f));
	}


	private void Start() {
		handCount = int.Parse(GameObject.Find("handNum").GetComponent<Text>().text);
		//cameraShake = GameObject.Find("Main Camera").GetComponent<cameraShake>();
	}

	Color randColor() {
		/*float h=181.0f/360.0f, s=0.85f, v=0.95f;
		//s = Random.Range(0.0f, 1.0f);
		return Color.HSVToRGB(h, s, v);*/
		return nowColor;
	}

	void handCountSeperate(int handcount, int flagnum) {
		string sub;
		if (handCount - flagnum <= 0) {
			sub = "(+10s)";
			nowColor = good;
			GameObject.Find("handNum").GetComponent<Text>().color = good;
		}
		else if (handCount - flagnum <= 5) {
			sub = "(+5s)";
			nowColor = middle;
			GameObject.Find("handNum").GetComponent<Text>().color = middle;
		}
		else {
			sub = "(+3s)";
			nowColor = bad;
			GameObject.Find("handNum").GetComponent<Text>().color = bad;
		}
	}


	// Update is called once per frame 
	void LateUpdate() {
		if (GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().pauseTrigger == false) {
			elapsedTime += Time.deltaTime;
		}
		if (elapsedTime - lastTouchTime >= durationToHint) {
			int hintDir = GetComponent<movePlayer>().nextPredictedDirection;

			if (hintDir == 0) {//down
				downHint.SetActive(true);
			}
			else if (hintDir == 1) {//left
				leftHint.SetActive(true);
			}
			else if (hintDir == 2) {//up
				upHint.SetActive(true);
			}
			else if (hintDir == 3) {//right
				rightHint.SetActive(true);
			}

		}
		else if(wantingDir==-1) {
			downHint.SetActive(false);
			leftHint.SetActive(false);
			upHint.SetActive(false);
			rightHint.SetActive(false);
		}


		//Debug.Log(ret);
		this.acc = Input.acceleration;
		//画面の対角線を境にして重力による加速度のベクトルで判断 
		float ru = Mathf.Atan2(1.0f, 1.0f); //Mathf.Atan2(Screen.height / 2.0f, Screen.width / 2.0f); 
		float lu = Mathf.Atan2(1.0f, -1.0f); //Mathf.Atan2(Screen.height / 2.0f, -Screen.width / 2.0f); 
		float ld = Mathf.Atan2(-1.0f, -1.0f); //Mathf.Atan2(-Screen.height / 2.0f, -Screen.width / 2.0f); 
		float rd = Mathf.Atan2(-1.0f, 1.0f); //Mathf.Atan2(-Screen.height / 2.0f, Screen.width / 2.0f); 

		float d = Mathf.Atan2(acc.y, acc.x);


		//Debug.Log(touchScript); 
		if (0 <= touchScript.GetTouch()) {
			lastTouch = touchScript.GetTouch();
		}
		bool trigger = GetComponent<mainPlayerAnimation>().moveTrigger;
		bool isPaused = GetComponent<mainPlayerAnimation>().pauseTrigger;



		//PC操作時はここをコメントアウト

		if (lastTouch == 4) { ret = 0; }//もし、下向きなら! 
		if (lastTouch == 1) { ret = 1; }//右
		if (lastTouch == 3) { ret = 2; }//上 
		if (lastTouch == 2) { ret = 3; }//左 
		if (lastTouch == 0) { ret = 5; }

		if (ret == 0 && !trigger && ret != beforeDirection && !isPaused) {//down
			lastTouchTime = Time.time;
			handCount++;
			int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 0;
			ifInput(ret);
	
			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (ret == 2 && !trigger && ret != beforeDirection && !isPaused) {//up
			lastTouchTime = Time.time;
			handCount++; int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 2;

			ifInput(ret);
			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (ret == 1 && !trigger && ret != beforeDirection && !isPaused) {
			lastTouchTime = Time.time;
			handCount++; int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 1; ifInput(ret);
			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (ret == 3 && !trigger && ret != beforeDirection && !isPaused) {
			lastTouchTime = Time.time;
			handCount++; int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 3;
			ifInput(ret);
			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
		}
		//

		if (Input.GetKeyDown(KeyCode.DownArrow) && !trigger && 0 != beforeDirection && !isPaused) {

			lastTouchTime = elapsedTime;
			handCount++;
			int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();

			ret = beforeDirection = 0;
			ifInput(ret);


			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && !trigger && 2 != beforeDirection && !isPaused) {
			lastTouchTime = elapsedTime;
			handCount++;
			int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 2; ifInput(ret);


			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && !trigger && 1 != beforeDirection && !isPaused) {

			lastTouchTime = elapsedTime;
			handCount++;
			int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 1; ifInput(ret);

			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && !trigger && 3 != beforeDirection && !isPaused) {

			lastTouchTime = elapsedTime;
			handCount++;
			int flagnum = int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text);
			string sub;
			handCountSeperate(handCount, flagnum);

			GameObject.Find("handNum").GetComponent<Text>().text = handCount.ToString();
			ret = beforeDirection = 3; ifInput(ret);

			moveAnimUp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			moveAnimDown.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
			//moveAnimDown.GetComponent<Image>().color = randColor();
			//moveAnimUp.GetComponent<Image>().color = randColor();
		}

	}



}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scoreFadeout : MonoBehaviour {
	public GameObject circle, scoreText, panel, scoreNum, highscoreText, homeButton, restartButton, rankingButton, shareButton;

	//Imageをつかったフェードアウトのリスト
	List<GameObject> imageGameObjectList = new List<GameObject>();
	List<Color> imageToColorList = new List<Color>();
	List<Color> imageStartColorList = new List<Color>();


	Animator titleStart;
	bool textFadeTrigger = false, panelActiveFalse = false, inputActiveFalse = false;
	public bool timeOutTrigger = false;
	public float fadeOutTime;
	float startTime;
	List<Text> textObject = new List<Text>();
	List<Color> textToColorList = new List<Color>();
	List<Color> textStartColorList = new List<Color>();
	public AnimationCurve curveIn;

	public IEnumerator animStart(float duration, float startScale) {
		float elapsedSecond = 0f;
		while (elapsedSecond < duration) {
			elapsedSecond += Time.deltaTime;
			float param = curveIn.Evaluate(elapsedSecond / duration) * startScale;
			GameObject.Find("timeAnimInMask").transform.localScale = new Vector3(param, param);
			yield return null;
		}
		GameObject.Find("timeAnimInMask").transform.localScale = new Vector3(startScale, startScale);
	}

	IEnumerator shareIEnum() {
		int score = GetComponent<showPoints>().retPoint();
		string tweetMapCode = GetComponent<data>().retMapcode();
		var shareText = "I got "+score+" points!\nThis is my mapcode! \nPaste it and play! #PERVERSEGAME\n["+tweetMapCode+"]";
		var shareUrl = "";
		var imagePath = Application.persistentDataPath + "/image.png";
		yield return null;
		SocialConnector.SocialConnector.Share(shareText, shareUrl, null);
	}
	public void shareOnClick() {
		StartCoroutine(shareIEnum());
	}

	IEnumerator shareRateMatch(string txt) {
		var imagePath = Application.persistentDataPath + "/image.png";
		yield return null;
		SocialConnector.SocialConnector.Share(txt, "", null);
	}

	public void mapcodeShare() {
		StartCoroutine(shareRateMatch("I clear this map!! Play it! #PERVERSEGAME\n["+PlayerPrefs.GetString("mapcodeModeString", "1ξβE√Σ#∞Φ∨⇔")+"]"));
	}

	// Use this for initialization
	void Start() {
		Application.targetFrameRate = 60;
	}

	void LateUpdate() {
		if (timeOutTrigger) {//imageのフェード
							 //0から1の進度状況を格納
			float param = (Time.time - startTime) / fadeOutTime;
			//もし移動中であれば
			if (param <= 1.0f) {
				for (int i = 0; i < imageGameObjectList.Count; i++) {
					//startの色とtoの色をparam:(1-param)に分けたときの色を代入
					Color updateColor = Color.Lerp(imageStartColorList[i], imageToColorList[i], param);
					imageGameObjectList[i].GetComponent<Image>().color = updateColor;
				}
			}
			//通り過ぎたら
			else {
				for (int i = 0; i < imageGameObjectList.Count; i++) {
					//ゲームオブジェクトの最終的な色を目的の色に合わせる
					imageGameObjectList[i].GetComponent<Image>().color = imageToColorList[i];
				}
				timeOutTrigger = false;
				imageToColorList.Clear();
				imageGameObjectList.Clear();
				imageStartColorList.Clear();
			}
		}
		if (textFadeTrigger) {//テキストのフェード
			float param = (Time.time - startTime) / fadeOutTime;
			if (param <= 1.0f) {
				for (int i = 0; i < textObject.Count; i++) {
					//startの色とtoの色をparam:(1-param)に分けたときの色を代入
					Color updateColor = Color.Lerp(textStartColorList[i], textToColorList[i], param);
					textObject[i].color = updateColor;
				}
			}
			else {
				for (int i = 0; i < textObject.Count; i++) {
					textObject[i].color = textToColorList[i];
				}
				textFadeTrigger = false;
				textToColorList.Clear();
				textStartColorList.Clear();
				textObject.Clear();
			}

		}

	}

	void imageAdd(GameObject o) {
		imageGameObjectList.Add(o);
		Color alphaToZero = o.GetComponent<Image>().color;
		imageStartColorList.Add(alphaToZero);
		alphaToZero.a = 1.0f;
		imageToColorList.Add(alphaToZero);
	}

	void textAdd(GameObject o, Color c) {
		textObject.Add(o.GetComponent<Text>());
		textStartColorList.Add(o.GetComponent<Text>().color);
		textToColorList.Add(c);
	}

	int culcSign = 0;
	public void timeOut() {
		bool isCompeteMode = GetComponent<data>().isCompeteMode;
		panel.SetActive(true);
		GameObject.Find("pointsText").GetComponent<data>().timeOverColorSet();
		//imageAdd(panel);
		StartCoroutine(animStart(.7f, 4f));

		if (SceneManager.GetActiveScene().name != "mapcode") {
			imageAdd(circle);

			textAdd(GameObject.Find("HighscoreNum"), new Color(0.96f, 0.96f, 0.96f, 1.0f));
		}
		imageAdd(restartButton);

		textAdd(scoreNum, new Color(1.0f, 1.0f, 0.3f, 1.0f));
		imageAdd(homeButton);

		textAdd(scoreText, new Color(0.96f, 0.96f, 0.96f, 1.0f));

		imageAdd(shareButton);



		startTime = Time.time;
		textFadeTrigger = true;
		timeOutTrigger = true;
		GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().pauseTrigger = true;
		int score = GetComponent<showPoints>().retPoint();

		int highscore = PlayerPrefs.GetInt("highscore", 0);
		PlayerPrefs.SetInt("highscore", Mathf.Max(score, highscore));
		PlayerPrefs.Save();

		if (isCompeteMode) {
            GameObject ev = GameObject.Find("EventSystem").gameObject;
            transceive Transceive = ev.GetComponent<transceive>();

			//レート計算
			//基本 : win:+30 lose:-30  draw:0
			//そこから補正値をひく
			//(-15~15の値) 自分のレートの方が高いほど大きくなる 逆も然り  

			int nowRate = PlayerPrefs.GetInt("rate", 0);

			int RATECHANGE = 30;
			int opponentRate = nowRate;
			opponentRate = getMap.retDMap.rate;
			int newRate = nowRate;
			int rateDiff = nowRate - opponentRate;
			

			//ここに代入

            //opoRate > myRate won -> +

            int correctionVal = (int)Mathf.Floor(rateDiff / 10);
            if (Mathf.Abs(correctionVal) > 25)
            {
                correctionVal = 25;
            }
            correctionVal = Mathf.Abs(correctionVal);

			int amountStage = GetComponent<data>().retNum();
			if (amountStage > score+2 ) {
				scoreText.GetComponent<Text>().text = "LOSE";
				culcSign = -1;
				Debug.Log(amountStage);
                Transceive.incMatchesPlayed();
			}
			if (amountStage == score+2 ) {
				scoreText.GetComponent<Text>().text = "DRAW";
				culcSign = 0;
                RATECHANGE = 0;
				Debug.Log("hello" + score);
                Transceive.incMatchesPlayed();

			}
			if (amountStage < score +2) {
				scoreText.GetComponent<Text>().text = "WIN";
                Transceive.incWins();
				culcSign = 1;

			}

            if(opponentRate<nowRate){
                correctionVal = -correctionVal;
            }

			//culc nowrate
            newRate = nowRate + (RATECHANGE) * culcSign+correctionVal+UnityEngine.Random.Range(0, 7)-3;


			//サーバーに保存
			GameObject.Find("EventSystem").gameObject.GetComponent<transceive>().updateRate(PlayerPrefs.GetInt("uid"), newRate);

            PlayerPrefs.SetInt("rate", newRate);
            PlayerPrefs.Save();


			if (nowRate <= newRate) {
				GameObject.Find("ScoreNum").GetComponent<Text>().text = nowRate + "\n↓\n" + newRate + "(+" + (newRate - nowRate) + ")";
			}
			else {
				GameObject.Find("ScoreNum").GetComponent<Text>().text = nowRate + "\n↓\n" + newRate + "(-" + -(newRate - nowRate) + ")";
			}
			if (newRate >= PlayerPrefs.GetInt("highestrate", 0)) {
                
				PlayerPrefs.SetInt("highestrate", newRate);
                PlayerPrefs.Save();

				GameObject.Find("ScoreNum").GetComponent<Text>().text += "\n\nHighest!!";
				//highestRateにきたぜ！
			}

			//端末に保存
			PlayerPrefs.SetInt("rate", newRate);
			PlayerPrefs.Save();
		}
		else if (SceneManager.GetActiveScene().name != "mapcode") {
			GameObject.Find("HighscoreNum").GetComponent<Text>().text = "Highscore : " + Mathf.Max(score, highscore);
			GameObject.Find("ScoreNum").GetComponent<Text>().text = score + "";
			if (score > highscore) {
				textAdd(highscoreText, new Color(1f, 1f, 1f, 1.0f));
			}
		}
	}

	public void rateMatchShare() {
		string txt="I ";
		if (culcSign == 1) {
			txt += "won ";
		}
		if (culcSign == 0) {
			txt += "draw ";
		}
		if (culcSign == -1) {
			txt += "lose ";
		}
		txt += "against ";
        //ここに対戦相手の名前を代入
        string battleUserName = getMap.vsUname;//getmapのstatic
		txt += battleUserName;



		txt += " in battle mode";
		if (culcSign == 1) {
			txt += "!!";
		}
		if (culcSign == 0) {
			txt += "!";
		}
		if (culcSign == -1) {
			txt += "...";
		}
		txt += "\n #PERVERSEGAME";

		StartCoroutine(shareRateMatch(txt));

	}

}

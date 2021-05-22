using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class titleButtonControler : MonoBehaviour {

	public GameObject startButton, tapEffect, panel, cross, Inputfield, Holdtext, mapcodecross, inputText, settingInsideButton, popOutPanel, blurEffect,popOutThing;
	public GameObject[] colorsList = new GameObject[7];

	//Imageをつかったフェードアウトのリスト
	List<GameObject> imageGameObjectList = new List<GameObject>();
	List<Color> imageToColorList = new List<Color>();
	List<Color> imageStartColorList = new List<Color>();
	public GameObject messageBoard;
	Animator titleStart;
	bool imageFadeoutTrigger = false, textFadeTrigger = false, panelActiveFalse = false, inputActiveFalse = false;
	public float fadeOutTime;
	float startTime;
	List<Text> textObject = new List<Text>();
	List<Color> textToColorList = new List<Color>();
	List<Color> textStartColorList = new List<Color>();

	public ToggleGroup toggleGroup1;

	string[] colorList = { "#035874", "#643d72", "#963E73", "#8c8b3c", "#55703a", "#656973" };
	static Color ToColor(string self) {
		var color = default(Color);
		if (!ColorUtility.TryParseHtmlString(self, out color)) {
			Debug.LogWarning("Unknown color code... " + self);
		}
		return color;
	}
	Color imageColor;
	public void toggleChange() {
		Toggle toggle1 = toggleGroup1.ActiveToggles().FirstOrDefault();
		PlayerPrefs.SetInt("colorNum", int.Parse(toggle1.name) - 1);

		if (PlayerPrefs.GetInt("colorNum", 1) == 6) {
			imageColor = ToColor(colorList[Random.Range(0, 6)]);
			GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = imageColor;return;

		}
		else {
			imageColor = ToColor(colorList[int.Parse(toggle1.name) - 1]);
		}
		GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = ToColor(colorList[int.Parse(toggle1.name) - 1]);
	}
	public GameObject outFade;
	IEnumerator loadScene(string name) {
		outFade.SetActive(true);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}

	IEnumerator wait(float duration) {
		yield return new WaitForSeconds(duration);
		mapcodeOpened = false;
	}
	//[2≧ΧΣβπ∫κ∽μΣΣ≦∪Σ#∞Ω]
	public void toMapcode() {

		string str = GameObject.Find("InputField").GetComponent<InputField>().text;
		if (str.IndexOf("[") >= 0) {
			str=str.Substring(str.IndexOf("[")+1);
		}
		if (str.IndexOf("]") >= 0) {
			str=str.Substring(0,str.IndexOf("]"));
		}
		if (str[0]-'0'>4|| str[0] - '0' < 0) { return; }
		PlayerPrefs.SetString("mapcodeModeString", str);
		PlayerPrefs.Save();
		StartCoroutine(loadScene("mapcode"));
	}

	public void whatIsMapcodeOnClick() {
		StartCoroutine(GameObject.Find("EventSystem").GetComponent<popout>().popoutAnimate(1.0f, messageBoard));
		GameObject.Find("text").GetComponent<Text>().text = "Mapcodes enable you to play maps others played!\n\nPlay the game and press SHARE to share your mapcode with your friends!";
		GameObject.Find("text").GetComponent<Text>().fontSize = 80;
	}

	// Use this for initialization
	void Start() {
		if (PlayerPrefs.GetInt("colorNum", 1) == 6) {
			imageColor = ToColor(colorList[Random.Range(0, 6)]);
		}
		else {
			imageColor = ToColor(colorList[PlayerPrefs.GetInt("colorNum", 1)]);
		}
		GameObject.Find(""+(1+PlayerPrefs.GetInt("colorNum", 1))).GetComponent<Toggle>().isOn = true;
		GameObject.Find("settingInsideButton").SetActive(false);
		GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor =imageColor;
		titleStart = startButton.GetComponent<Animator>();
		Application.targetFrameRate = 60;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GameObject tap = tapEffect;
			tap = Instantiate(tapEffect, tap.transform.position, tap.transform.rotation);
			//子オブジェクトにする
			tap.transform.SetParent(transform);
			tap.transform.position = Input.mousePosition;
			//tap.transform.SetAsFirstSibling();
		}

		if (imageFadeoutTrigger) {//imageのフェード
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
				imageFadeoutTrigger = false;
				imageToColorList.Clear();
				imageGameObjectList.Clear();
				imageStartColorList.Clear();
				if (panelActiveFalse) {
					panel.SetActive(false);
					panelActiveFalse = false;

					settingInsideButton.SetActive(false);
					popOutPanel.SetActive(false);
				}
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
				if (inputActiveFalse) {
					inputActiveFalse = false;
					Inputfield.SetActive(false);
					Holdtext.SetActive(false);
				}
			}

		}


	}
	public Animator moveUp,moveUpRoof,startButtonIn;
	public GameObject tutorialHand;
	bool startButtonTrigger = false;
	IEnumerator waitOff(float duration) {
		yield return new WaitForSeconds(duration);
		if (PlayerPrefs.GetInt("uid", -1) == -1) {
			tutorialHand.SetActive(true);
		}
		GameObject.Find("startButton").SetActive(false);
	}
	public void startButtonOnclick() {
		if (startButtonTrigger) { return; }
		startButtonTrigger = true;
		moveUp.SetTrigger("moveUpTrigger");
		moveUpRoof.SetTrigger("moveUpTrigger");
		startButtonIn.SetTrigger("moveUpTrigger");
		GameObject.Find("startButtonOut").SetActive(false);
		StartCoroutine(waitOff(1f));

		/*imageFadeoutTrigger = true;
		startTime = Time.time;
		panel.SetActive(true);

		imageGameObjectList.Add(panel);
		imageStartColorList.Add(panel.GetComponent<Image>().color);

		//panelToColorに目標の色を代入すればその色に変わる！！
		Color toColor = GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor;
		toColor.a = 1;
		imageToColorList.Add(toColor);*/
	}

	public void settingButtonOnClick() {
		startTime = Time.time;
		panel.SetActive(true);
		imageFadeoutTrigger = true;
		cross.SetActive(true);
		mapcodecross.SetActive(false);
		imageGameObjectList.Add(panel);
		imageStartColorList.Add(panel.GetComponent<Image>().color);
		imageToColorList.Add(new Color(127.0f / 255.0f, 56.0f / 255.0f, 98.0f / 255.0f, 1.0f));

		imageGameObjectList.Add(cross);
		imageStartColorList.Add(cross.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		textObject.Add(GameObject.Find("SettingInsideText").GetComponent<Text>());
		textStartColorList.Add(GameObject.Find("SettingInsideText").GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));
		textFadeTrigger = true;

		settingInsideButton.SetActive(true);
		for (int i = 0; i < 7; i++) {
			imageGameObjectList.Add(colorsList[i]);
			Color fr = new Color(1f, 1f, 1f, 0f);
			imageStartColorList.Add(fr);
			imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));
		}
		imageGameObjectList.Add(GameObject.Find("tutorialButton"));
		imageStartColorList.Add(new Color(1f, 1f, 1f, 0f));
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		cross.GetComponent<Image>().raycastTarget = true;
	}

	public void settingCrossOnClick() {
		if (panel.activeSelf == false) {
			return;
		}
		startTime = Time.time;
		panelActiveFalse = true;
		imageFadeoutTrigger = true;
		textFadeTrigger = true;

		imageGameObjectList.Add(panel);
		imageStartColorList.Add(panel.GetComponent<Image>().color);
		imageToColorList.Add(new Color(127.0f / 255.0f, 56.0f / 255.0f, 98.0f / 255.0f, 0.0f));

		imageGameObjectList.Add(cross);
		imageStartColorList.Add(cross.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));

		textObject.Add(GameObject.Find("SettingInsideText").GetComponent<Text>());
		textStartColorList.Add(GameObject.Find("SettingInsideText").GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));

		cross.GetComponent<Image>().raycastTarget = false;

		for (int i = 0; i < 7; i++) {
			imageGameObjectList.Add(colorsList[i]);
			Color fr = new Color(1f, 1f, 1f, 1f);
			imageStartColorList.Add(fr);
			imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));
		}

		imageGameObjectList.Add(GameObject.Find("tutorialButton"));
		imageStartColorList.Add(new Color(1f, 1f, 1f, 1f));
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, .0f));
	}

	bool mapcodeOpened = false;
	public Color mapcodePanel;
	public void mapcodeButtonOnClick() {
        if (getMap.isLoading) return;

		if (PlayerPrefs.GetInt("uid", -1) == -1) {
            Debug.Log("your user data was not found,plase register first of all");
			return;
		}
		if (mapcodeOpened) { Debug.Log("hello"); return; }
		mapcodeOpened = true;
		startTime = Time.time;
		panel.SetActive(true);
		imageFadeoutTrigger = true;
		mapcodecross.SetActive(true);
		Inputfield.SetActive(true);
		Holdtext.SetActive(true);
		popOutPanel.SetActive(true);
		textFadeTrigger = true;
		cross.SetActive(false);


		//blurEffect.GetComponent<Image>().color = imageColor;

		imageGameObjectList.Add(panel);
		imageStartColorList.Add(panel.GetComponent<Image>().color);
		imageToColorList.Add(mapcodePanel);

		imageGameObjectList.Add(mapcodecross);
		imageStartColorList.Add(cross.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		imageGameObjectList.Add(Inputfield);
		imageStartColorList.Add(Inputfield.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		imageGameObjectList.Add(popOutPanel);
		imageStartColorList.Add(popOutPanel.GetComponent<Image>().color);
		imageToColorList.Add(new Color(194.0f / 255.0f, 89.0f / 255.0f, 98.0f / 255.0f, 1.0f));

		textObject.Add(Holdtext.GetComponent<Text>());
		textStartColorList.Add(Holdtext.GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		textObject.Add(GameObject.Find("MapcodeInsideText").GetComponent<Text>());
		textStartColorList.Add(GameObject.Find("MapcodeInsideText").GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		textObject.Add(inputText.GetComponent<Text>());
		textStartColorList.Add(inputText.GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 1.0f));

		//StartCoroutine(GetComponent<popout>().popoutAnimate(.4f, popOutThing));
		StartCoroutine(wait(.41f));
		mapcodecross.GetComponent<Image>().raycastTarget = true;
		
	}

	public void mapcodeCrossOnClick() {
		if (panel.activeSelf == false||mapcodeOpened) {
			Debug.Log("hello");
			return;
		}
		mapcodeOpened = true;
		startTime = Time.time;
		panelActiveFalse = true;
		inputActiveFalse = true;
		imageFadeoutTrigger = true;
		textFadeTrigger = true;

		imageGameObjectList.Add(panel);
		imageStartColorList.Add(panel.GetComponent<Image>().color);
		imageToColorList.Add(new Color(127.0f / 255.0f, 56.0f / 255.0f, 98.0f / 255.0f, 0.0f));

		imageGameObjectList.Add(mapcodecross);
		imageStartColorList.Add(cross.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));

		imageGameObjectList.Add(Inputfield);
		imageStartColorList.Add(Inputfield.GetComponent<Image>().color);
		imageToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));

		imageGameObjectList.Add(popOutPanel);
		imageStartColorList.Add(popOutPanel.GetComponent<Image>().color);
		imageToColorList.Add(new Color(194.0f / 255.0f, 89.0f / 255.0f, 98.0f / 255.0f, 0.0f));

		textObject.Add(Holdtext.GetComponent<Text>());
		textStartColorList.Add(Holdtext.GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));


		textObject.Add(GameObject.Find("MapcodeInsideText").GetComponent<Text>());
		textStartColorList.Add(GameObject.Find("MapcodeInsideText").GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));

		textObject.Add(inputText.GetComponent<Text>());
		textStartColorList.Add(inputText.GetComponent<Text>().color);
		textToColorList.Add(new Color(1.0f, 1.0f, 1.0f, 0.0f));
		mapcodecross.GetComponent<Image>().raycastTarget = false;

		//StartCoroutine(GetComponent<popout>().popfadeAnimate(.4f, popOutThing));
		StartCoroutine(wait(.41f));
		
	}
}

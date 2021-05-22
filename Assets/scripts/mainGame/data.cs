using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-4)]
public class data : MonoBehaviour {
	public static List<string> mapcodeList = new List<string>();
	public static List<string> mapcodeVisualList = new List<string>();
	codemaker codeScript;
	public int colorNum;
	public int difficulty = 2;//0~2で表す
	public static int howmanyBotMapCnt;
	public bool isCompeteMode;
	public bool isTutorialMode;
	public bool isMapCodeMode;
	string userName;
	int userRate;

	public string retMapcode() {
		return mapcodeList[mapcodeList.Count - 1];
	}

	string[] colorLight = { "#0ba6dc", "#c6a6e1", "#f9c4ee", "#e6e3a9", "#c5e1a5", "#d5dcdd" }
	, colorDark = { "#0a8fbe", "#b37dc7", "#d49bbe", "#d8ca64", "#94af76", "#a2a3a6" },
		colorRotate = { "#7386e6", "#985B97", "#985B97", "#F6A742", "#A3CB47", "#5A5A5A" };

	IEnumerator vsWhoAnimate(float duration) {
		yield return new WaitForSeconds(1.5f);
		float startTime = 0f;
		GameObject vsWho = GameObject.Find("vsWho");
		float startScale = vsWho.transform.localScale.x;
		while (startTime < duration) {
			startTime += Time.deltaTime;
			float per = 1f - startTime / duration;
			vsWho.transform.localScale = new Vector3(per * startScale, per * startScale);
			yield return null;
		}
		vsWho.SetActive(false);
	}

	public int retNum() {
		return mapcodeVisualList.Count;
	}

	static Color ToColor(string self) {
		var color = default(Color);
		if (!ColorUtility.TryParseHtmlString(self, out color)) {
			Debug.LogWarning("Unknown color code... " + self);
		}
		return color;
	}
	public List<string> ret() {
		return mapcodeList;
	}
	public void timeOverColorSet() {
		GameObject.Find("timeAnimInMask").GetComponent<Image>().color = ToColor(colorDark[colorNum]);
		if (SceneManager.GetActiveScene().name != "mapcode") {
			GameObject.Find("circle").GetComponent<Image>().color = ToColor(colorLight[colorNum]);
		}
	}
	private void Awake() {
		int nowPoint = GetComponent<showPoints>().retPoint();
		if (nowPoint >= 1) {
			Destroy(GameObject.Find("in"));
		}
		if (nowPoint < 5) {
			difficulty = 0;
		}
		else if (nowPoint < 15) {
			difficulty = 1;
		}
		else if (nowPoint < 25) {
			difficulty = 3;
		}
		else {
			difficulty = 2;
		}
		if (nowPoint == 0) {
			mapcodeList.Clear();
		}
		colorNum = PlayerPrefs.GetInt("colorNum", 0);
		if (colorNum == 6) {
			colorNum = Random.Range(0, 5);
		}
		GameObject.Find("scoreWallBig").GetComponent<Image>().color = ToColor(colorDark[colorNum]);
		GameObject.Find("scoreWallSmall").GetComponent<Image>().color = ToColor(colorLight[colorNum]);
		GameObject.Find("scoreWallSmall2").GetComponent<Image>().color = ToColor(colorRotate[colorNum]);
		GameObject.Find("goalAnimInMask").GetComponent<Image>().color = ToColor(colorLight[colorNum]);
		GameObject.Find("goalAnimInMask2").GetComponent<Image>().color = ToColor(colorLight[colorNum]);

		if (isCompeteMode) {

			if (nowPoint == 0) {
				GameObject.Find("vsWho").GetComponent<Image>().color = ToColor(colorDark[colorNum]);
				//ここで代入
				//Debug.Log("name " + getMap.vsUname);
				Debug.Log("rate " + getMap.retDMap.rate);

				userName = getMap.vsUname;
				userRate = getMap.retDMap.rate;

				GameObject.Find("vsWhoText").GetComponent<Text>().text = "You\n\nVS\n\n" + userName + "\n(rate:" + userRate + ")";
				StartCoroutine(vsWhoAnimate(0.7f));
			}
			else {
				GameObject.Find("vsWho").SetActive(false);
			}

		}

		//sample
		//mapcodeVisualList.Add("3Σβ4βΣ┃4β─ΩΣ≡＞─βκκ∧Υ≠κ≠∈ΤΣΣ3Υ≦┓λΔΣ#⊂θ∋┻aq");
		if (isCompeteMode) {

			GameObject.Find("Canvas").GetComponent<automaticgenerator>().isCompeteMode = true;
			if (mapcodeVisualList.Count == nowPoint) {
				//全部クリアした
				GetComponent<scoreFadeout>().timeOut();
			}
			GameObject.Find("Canvas").GetComponent<automaticgenerator>().mapCode = mapcodeVisualList[nowPoint];
		}
		if (isTutorialMode) {
			GameObject.Find("Canvas").GetComponent<automaticgenerator>().isTutorialMode = true;
			GameObject.Find("Canvas").GetComponent<automaticgenerator>().mapCode = "1γβΧΚΣ#∝∞";
		}
		if (isMapCodeMode) {

			GameObject.Find("Canvas").GetComponent<automaticgenerator>().isTutorialMode = true;
			GameObject.Find("Canvas").GetComponent<automaticgenerator>().mapCode = PlayerPrefs.GetString("mapcodeModeString", "1ξβE√Σ#∞Φ∨⇔");
			difficulty = int.Parse(PlayerPrefs.GetString("mapcodeModeString", "1ξβE√Σ#∞Φ∨⇔")[0].ToString()) - 1;
		}
	}
	public void addCode() {
		codeScript = GetComponent<codemaker>();
		mapcodeList.Add(codeScript.codegenerate(difficulty));
		foreach (var a in mapcodeList) {
			Debug.Log(a);
		}
	}
	public void botGameClear() {
		howmanyBotMapCnt++;
		Debug.Log(howmanyBotMapCnt);
		if (howmanyBotMapCnt == 5) {
			//ここにうpする処理を書いてください
			Debug.Log("otaku");
			SceneManager.LoadScene("Title");
		}
		else {
			addCode();

			GameObject.Find("playerObjectUp").GetComponent<mainPlayerAnimation>().BotIfGoal();
		}
	}
	void Start() {
		addCode();
	}
	// Update is called once per frame
	void Update() {
	}
	public string[] getPlayedMapcode() {
		return mapcodeList.ToArray();
	}
	public void isClearedMaybeNot() {
		if (mapcodeVisualList.Count-1 == GetComponent<showPoints>().retPoint()) {
			//全部クリアした
			GetComponent<scoreFadeout>().timeOut();
		}
		else {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
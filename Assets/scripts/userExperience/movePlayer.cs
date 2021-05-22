using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movePlayer : MonoBehaviour {

	public int goaldownx, goalupx, goaldowny, goalupy;
	int[,] map;
	int nowupx, nowdownx, nowupy, nowdowny;
	float ofsX, ofsY, ofsScale;
	public bool moveTrigger;
	public int moveDirection;
	public int nextPredictedDirection;
	public bool goalTrigger = false;
	// Use this for initialization
	public setBlock setBlock;
	public mainPlayerAnimation playerup, playerdown;
	GameObject upObject, downObject;
	public GameObject resetSuggest;
	string scenename;

	public GameObject outFade;
	IEnumerator loadScene(string name) {
		outFade.SetActive(true);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}
	public void homeButtonOnClick() {
		GameObject.Find("pointsText").GetComponent<timeCounter>().reset();
		GameObject.Find("pointsText").GetComponent<showPoints>().reset();
		StartCoroutine(loadScene("Title"));
	}

	public void gameRestart() {
		GameObject.Find("pointsText").GetComponent<timeCounter>().reset();
		GameObject.Find("pointsText").GetComponent<showPoints>().reset();
		StartCoroutine(loadScene(SceneManager.GetActiveScene().name));
	}

	public void ResetOnClick() {
		if (GetComponent<mainPlayerAnimation>().pauseTrigger == true && SceneManager.GetActiveScene().name != "mapcode") {

			GameObject.Find("pointsText").GetComponent<timeCounter>().reset();
			GameObject.Find("pointsText").GetComponent<showPoints>().reset();
			StartCoroutine(loadScene("Title"));
		}
		else  {
			resetSuggest.SetActive(false);
			if (SceneManager.GetActiveScene().name == "Tutorial") {
				if (upObject.GetComponent<tutorialGetAcc>().wantingDir == -1) {
					upObject.GetComponent<tutorialGetAcc>().ret = 4022;
				}
				else {
					return;
				}

			}
			else {
				upObject.GetComponent<GetAcc>().ret = 4022;
			}
			int difficulty = GameObject.Find("pointsText").GetComponent<data>().difficulty;
			if (difficulty == 0) {
				nowupx = 7;
				nowdownx = 5;
				nowupy = 3;
				nowdowny = 5;
			}
			if (difficulty == 1) {
				nowupx = 9;
				nowdownx = 7;
				nowupy = 7;
				nowdowny = 9;
			}
			if (difficulty == 2) {
				nowupx = 13;
				nowdownx = 11;
				nowupy = 11;
				nowdowny = 13;
			}
			if (difficulty == 3) {
				nowupx = 11;
				nowupy = 9;
				nowdownx = 9;
				nowdowny = 11;
			}
			if (SceneManager.GetActiveScene().name == "Tutorial") {
				if (upObject.GetComponent<tutorialGetAcc>().wantingDir == -1) {
					upObject.GetComponent<tutorialGetAcc>().beforeDirection = 404;
				}
				else {
					return;
				}
			}
			else {
				upObject.GetComponent<GetAcc>().beforeDirection = 404;
			}
			upObject.transform.position = new Vector3((float)(nowupx + 1 + 0.5) * 100 * ofsScale, (float)(nowupy + 0.5 + ofsY) * 100 * ofsScale, 0);
			downObject.transform.position = new Vector3((float)(nowdownx + 1 + 0.5) * 100 * ofsScale, (float)(nowdowny + 0.5 + ofsY) * 100 * ofsScale, 0);
			int cnt = GameObject.Find("Canvas").GetComponent<countToGoal>().cnt(nowupx, nowupy, nowdownx, nowdowny);
			Debug.Log(cnt);
			nextPredictedDirection = GameObject.Find("Canvas").GetComponent<countToGoal>().whichDirectionToGo(nowupx, nowupy, nowdownx, nowdowny, cnt);
			string[] directionString = { "下", "左", "上", "右" };
			Debug.Log(directionString[nextPredictedDirection]);
		}
	}

	int karix, kariy;
	int Selectrange(int x, int y, int nowx, int nowy)//このx,yはx方向にどれだけ、y方向も同様なので、通常x,y=1,0・0,1・-1,0・0,-1
   {
		int count = 0;
		while (true) {
			if (map[nowy + y, nowx + x] == 3) {
				karix = nowx; kariy = nowy;
				return count;//壁に当たったら、何マス行けたかを返す
			}
			else if (map[nowy + y, nowx + x] == 1) {
				karix = nowx; kariy = nowy;
				//Debug.Log((nowy + y) + " " + (nowx + x) + " " + map[nowy + y, nowx + x]);
				return count;//壁に当たったら、何マス行けたかを返す
			}
			nowx += x; nowy += y; count++;
		}
	}

	void selectDirectionandrange(int Direction)//このx,yはgenzaix,genzaiyの意味です。すみません
	{
		int up, down;
		//方向を把握していないので、適当に0を上、1を右、2を下、3を左にします
		if (Direction == 2)//上向きの重力
		{
			up = Selectrange(0, 1, nowupx, nowupy);
			nowupx = karix; nowupy = kariy;
			down = Selectrange(0, -1, nowdownx, nowdowny);
			nowdownx = karix; nowdowny = kariy;
		}
		else if (Direction == 1)//右
		{
			up = Selectrange(1, 0, nowupx, nowupy);
			nowupx = karix; nowupy = kariy;
			down = Selectrange(-1, 0, nowdownx, nowdowny);
			nowdownx = karix; nowdowny = kariy;

		}
		else if (Direction == 0)//下
		{
			up = Selectrange(0, -1, nowupx, nowupy);
			nowupx = karix; nowupy = kariy;
			down = Selectrange(0, 1, nowdownx, nowdowny);
			nowdownx = karix; nowdowny = kariy;
		}
		else//左
		{
			up = Selectrange(-1, 0, nowupx, nowupy);
			nowupx = karix; nowupy = kariy;
			down = Selectrange(1, 0, nowdownx, nowdowny);
			nowdownx = karix; nowdowny = kariy;
		}
		playerup.toPos = new Vector3((float)(nowupx + 1 + 0.5) * 100 * ofsScale, (float)(nowupy + 0.5 + ofsY) * 100 * ofsScale, 0);
		playerdown.toPos = new Vector3((float)(nowdownx + 1 + 0.5) * 100 * ofsScale, (float)(nowdowny + 0.5 + ofsY) * 100 * ofsScale, 0);
		//これで代入おーわり。あとはこれに従っていどうして
		if (map[nowupy, nowupx] == 2) {
			upObject.transform.Find("player").GetComponent<Image>().color = new Color(0.04f, 0.52f, 0.83f);
		}
		else {
			upObject.transform.Find("player").GetComponent<Image>().color = new Color(0.32f, 0.96f, 0.96f);
		}
		if (map[nowdowny, nowdownx] == 2) {
			downObject.transform.Find("player").GetComponent<Image>().color = new Color(0.77f, 0.30f, 0.76f);
		}
		else {
			downObject.transform.Find("player").GetComponent<Image>().color = new Color(0.63f, 0.48f, 0.97f);
		}

		if (map[nowupy, nowupx] == 2 && map[nowdowny, nowdownx] == 2) {
			goalTrigger = true;
		}
		//Debug.Log(GameObject.Find("Canvas").GetComponent<countToGoal>().cnt(nowupx, nowupy, nowdownx, nowdowny));
		else if (GameObject.Find("Canvas").GetComponent<countToGoal>().cnt(nowupx, nowupy, nowdownx, nowdowny) == -1) {
			resetSuggest.SetActive(true);
			nextPredictedDirection = -1;
		}
		else {
			int cnt = GameObject.Find("Canvas").GetComponent<countToGoal>().cnt(nowupx, nowupy, nowdownx, nowdowny);
			Debug.Log(cnt);
			nextPredictedDirection = GameObject.Find("Canvas").GetComponent<countToGoal>().whichDirectionToGo(nowupx, nowupy, nowdownx, nowdowny, cnt);
			string[] directionString = { "下", "左", "上", "右" };
			Debug.Log(directionString[nextPredictedDirection]);

		}
	}


	void Start() {
		map = GameObject.Find("Canvas").GetComponent<automaticgenerator>().map;
		scenename = SceneManager.GetActiveScene().name;
		int difficulty = GameObject.Find("pointsText").GetComponent<data>().difficulty;
		if (difficulty == 0) {
			nowupx = 7;
			nowdownx = 5;
			nowupy = 3;
			nowdowny = 5;
		}
		if (difficulty == 1) {
			nowupx = 9;
			nowdownx = 7;
			nowupy = 7;
			nowdowny = 9;
		}
		if (difficulty == 2) {
			nowupx = 13;
			nowdownx = 11;
			nowupy = 11;
			nowdowny = 13;
		}
		if (difficulty == 3) {
			nowupx = 11;
			nowupy = 9;
			nowdownx = 9;
			nowdowny = 11;
		}
		ofsX = setBlock.ofsX;
		ofsY = setBlock.ofsY;
		ofsScale = setBlock.ofsScale;
		upObject = GameObject.Find("playerObjectUp");
		downObject = GameObject.Find("playerObjectDown");
		upObject.transform.position = new Vector3((float)(nowupx + 1 + 0.5) * 100 * ofsScale, (float)(nowupy + 0.5 + ofsY) * 100 * ofsScale, 0);
		downObject.transform.position = new Vector3((float)(nowdownx + 1 + 0.5) * 100 * ofsScale, (float)(nowdowny + 0.5 + ofsY) * 100 * ofsScale, 0);

		/*Debug.Log("  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7");
		for (int i = 0; i < 17; i++) {
			string a = ""; a += i.ToString(); a += " ";
			for (int j = 0; j < 17; j++) {
				if (map[i, j] == 1) { a += "1"; 
				else { a += "0"; }
			}
			Debug.Log(a);
		}*/
		int cnt = GameObject.Find("Canvas").GetComponent<countToGoal>().cnt(nowupx, nowupy, nowdownx, nowdowny);
		//Debug.Log(cnt);
		nextPredictedDirection = GameObject.Find("Canvas").GetComponent<countToGoal>().whichDirectionToGo(nowupx, nowupy, nowdownx, nowdowny, cnt);
		string[] directionString = { "下", "左", "上", "右" };
		Debug.Log(directionString[nextPredictedDirection]);
	}

	// Update is called once per frame
	void Update() {
		if (moveTrigger) {
			float plus = 0.5f;
			upObject.GetComponent<mainPlayerAnimation>().startPos = upObject.transform.position;
			downObject.GetComponent<mainPlayerAnimation>().startPos = downObject.transform.position;
			//upObject.GetComponent<mainPlayerAnimation>().startPos = new Vector3((float)(nowupx + 1 + 0.5) * 100 * ofsScale, (float)(nowupy + 0.5 + ofsY) * 100 * ofsScale, 0);
			//downObject.GetComponent<mainPlayerAnimation>().startPos = new Vector3((float)(nowdownx + 1 + 0.5) * 100 * ofsScale, (float)(nowdowny + 0.5 + ofsY) * 100 * ofsScale, 0);
			selectDirectionandrange(moveDirection);
			moveTrigger = false;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class mainPlayerAnimation : MonoBehaviour {


	public Vector3 toPos;//目標地点
	public Vector3 startPos;//アニメーション開始時の位置
	public float timeToMove, lateInTime, lateOutTime;//何秒間で動くか、真ん中の尻尾の遅延時間、両端の尻尾の遅延時間
	GameObject playerTail0, player, playerTail1, playerTail2;//真ん中の尻尾、キューブ、端っこの尻尾2つ
	public GameObject pausePanel;
	public float startTime;
	float dist, tailStartScaleX;
	float tailStartScaleY;
	Image pauseImage, retryImage;
	[SerializeField]
	AnimationCurve curve;
	public bool moveTrigger;
	public int direction;
	public Sprite state, blur, play, pause, home, retry;
	private Image playerImage;
	public bool pauseTrigger = false, upstop = false, downstop = false;

    public AudioClip goalSound;

	IEnumerator ifGoal() {
        AudioSource AS = GetComponent<AudioSource>();
        AS.clip = goalSound;
        AS.volume = 0.7F;
        AS.pitch = 1F;
        AS.Play();

		pauseTrigger = true;
		if (SceneManager.GetActiveScene().name == "Tutorial") {
			GameObject.Find("EventSystem").GetComponent<tutorialSystem>().isOkClicked = true;
		}
		else if (SceneManager.GetActiveScene().name == "mapcode") {
			GameObject.Find("pointsText").GetComponent<scoreFadeout>().timeOut();
		}
		else {
			GameObject pointText = GameObject.Find("pointsText");
			int randomRange=Random.Range(0, 4);
			pointText.GetComponent<timeCounter>().handImageNumSet(randomRange);
			pointText.GetComponent<timeCounter>().timeCount(GetComponent<GetAcc>().handCount, int.Parse(GameObject.Find("flagNum").GetComponent<Text>().text));
			pointText.GetComponent<showPoints>().goal();
			StartCoroutine(GameObject.Find("goalAnimInMask").GetComponent<goalMaskAnim>().animEnd(0.7f, 2.0f, 0.4f));
			StartCoroutine(GameObject.Find("goalAnimInMask2").GetComponent<goalMaskAnim>().animEnd(0.7f, 2.0f, 0.4f));


			yield return new WaitForSeconds(1.15f);
			if (SceneManager.GetActiveScene().name == "RateMatch") {
				GameObject.Find("pointsText").GetComponent<data>().isClearedMaybeNot();
			}
			else {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
	public void BotIfGoal() {
		pauseTrigger = true;
		if (SceneManager.GetActiveScene().name == "Tutorial") {
			GameObject.Find("EventSystem").GetComponent<tutorialSystem>().isOkClicked = true;
		}
		else if (SceneManager.GetActiveScene().name == "mapcode") {
			GameObject.Find("pointsText").GetComponent<scoreFadeout>().timeOut();
		}
		else {
			GameObject pointText = GameObject.Find("pointsText");
			int randomRange = Random.Range(0, 4);
			pointText.GetComponent<showPoints>().goal();			
			if (SceneManager.GetActiveScene().name == "RateMatch") {
				GameObject.Find("pointsText").GetComponent<data>().isClearedMaybeNot();
			}
			else {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
	public GameObject outFade;
	IEnumerator loadScene(string name) {
		outFade.SetActive(true);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}


	public void pauseOnClick() {
		if(SceneManager.GetActiveScene().name=="Debug"&& GameObject.Find("pointsText").GetComponent<timeCounter>().getElapsedSeconds() <= 3f) {
			return;
		}
		if (pauseTrigger == false) {
			pausePanel.SetActive(true);
			pauseImage.sprite = play;
			retryImage.sprite = home;
		}
		else {
			pausePanel.SetActive(false);
			pauseImage.sprite = pause;
			retryImage.sprite = retry;
		}
		pauseTrigger = !pauseTrigger;
		
	}

	public void goHome() {
		GameObject.Find("pointsText").GetComponent<timeCounter>().reset();
		GameObject.Find("pointsText").GetComponent<showPoints>().reset();
		StartCoroutine(loadScene("Title"));
	}

	void LateUpdate() {


		if (moveTrigger) {

			if (direction == 3 || direction == 1) {//横方向の移動

				dist = Vector3.Distance(startPos, toPos);
				float timeElapsed = Time.time - startTime;
				if (Time.time - startTime > timeToMove + lateInTime) {
					transform.position = toPos;
					startPos = transform.position;
					moveTrigger = false;
					if (gameObject.name == "playerObjectUp" && GetComponent<movePlayer>().goalTrigger) {
						StartCoroutine(ifGoal());
					}
				}

				if (timeElapsed < 0.0f) {
					timeElapsed = 0.0000000001f;
				}

				float positionParamater = timeElapsed / timeToMove;
				float curvePos = curve.Evaluate(positionParamater);
				player.transform.position = Vector3.Lerp(startPos, toPos, curvePos);
				playerTail0.transform.position = Vector3.Lerp(startPos, toPos, curvePos);

				if (positionParamater > 0.55 || Vector3.Distance(player.transform.position, toPos) < 0.01) {//0.55をすぎたらキューブのテクスチャを戻す
					playerImage.sprite = state;
				}
				else {
					playerImage.sprite = blur;//そうでなければブラーをかける
				}

				float tailOutPosParamater = Mathf.Max(0.0f, (timeElapsed - lateOutTime)) / timeToMove;
				float tailOutCurvePos = curve.Evaluate(tailOutPosParamater);
				//playerTail1.transform.position = new Vector3((Mathf.Lerp(startPos.x, toPos.x, tailOutCurvePos) + player.transform.position.x) / 2, playerTail1.transform.position.y, 0);
				//playerTail2.transform.position = new Vector3((Mathf.Lerp(startPos.x, toPos.x, tailOutCurvePos) + player.transform.position.x) / 2, playerTail2.transform.position.y, 0);

				float tailInPosParamater = Mathf.Max(0.0f, (timeElapsed - lateInTime)) / timeToMove;
				float tailInCurvePos = curve.Evaluate(tailInPosParamater);
				playerTail0.transform.position = new Vector3((Mathf.Lerp(startPos.x, toPos.x, tailInCurvePos) + player.transform.position.x) / 2, playerTail0.transform.position.y, 0);

				playerTail0.transform.localScale = new Vector3(Mathf.Abs(player.transform.localPosition.x - playerTail0.transform.localPosition.x) * 2.0f * 0.75f, tailStartScaleY, 1.0f);
				//playerTail1.transform.localScale = new Vector3(tailStartScaleX + Mathf.Abs(player.transform.position.x - playerTail1.transform.position.x) * 2, playerTail1.transform.localScale.y, 1.0f);
				//playerTail2.transform.localScale = new Vector3(tailStartScaleX + Mathf.Abs(player.transform.position.x - playerTail2.transform.position.x) * 2, playerTail1.transform.localScale.y, 1.0f);


			}
			if (direction == 0 || direction == 2) { //縦方向の移動
				dist = Vector3.Distance(startPos, toPos);

				float timeElapsed = Time.time - startTime;
				if (Time.time - startTime > timeToMove + lateInTime) {
					transform.position = toPos;
					startPos = transform.position;

					moveTrigger = false;
					if (gameObject.name == "playerObjectUp" && GetComponent<movePlayer>().goalTrigger) {
						StartCoroutine(ifGoal());
					}
				}

				if (timeElapsed < 0.0f) {
					timeElapsed = 0.0000000001f;
				}

				float positionParamater = timeElapsed / timeToMove;
				float curvePos = curve.Evaluate(positionParamater);
				player.transform.position = Vector3.Lerp(startPos, toPos, curvePos);

				if (positionParamater > 0.55 || Vector3.Distance(player.transform.position, toPos) < 0.01) {
					playerImage.sprite = state;
				}
				else {
					playerImage.sprite = blur;
				}

				float tailOutPosParamater = Mathf.Max(0.0f, (timeElapsed - lateOutTime)) / timeToMove;
				float tailOutCurvePos = curve.Evaluate(tailOutPosParamater);
				//playerTail1.transform.position = new Vector3(playerTail1.transform.position.x, (Mathf.Lerp(startPos.y, toPos.y, tailOutCurvePos) + player.transform.position.y) / 2, 0);
				//playerTail2.transform.position = new Vector3(playerTail2.transform.position.x, (Mathf.Lerp(startPos.y, toPos.y, tailOutCurvePos) + player.transform.position.y) / 2, 0);

				float tailInPosParamater = Mathf.Max(0.0f, (timeElapsed - lateInTime)) / timeToMove;
				float tailInCurvePos = curve.Evaluate(tailInPosParamater);
				playerTail0.transform.position = new Vector3(playerTail0.transform.position.x, (Mathf.Lerp(startPos.y, toPos.y, tailInCurvePos) + player.transform.position.y) / 2, 0);


				playerTail0.transform.localScale = new Vector3(tailStartScaleX, Mathf.Abs(player.transform.localPosition.y - playerTail0.transform.localPosition.y) * 2.0f * 0.75f, 1.0f);
				//playerTail1.transform.localScale = new Vector3(playerTail1.transform.localScale.x, tailStartScaleY + Mathf.Abs(player.transform.position.y - playerTail1.transform.position.y) * 2, 1.0f);
				//playerTail2.transform.localScale = new Vector3(playerTail1.transform.localScale.x, tailStartScaleY + Mathf.Abs(player.transform.position.y - playerTail2.transform.position.y) * 2, 1.0f);
			}
		}
	}

	void Start() {
		pauseImage = GameObject.Find("pause").GetComponent<Image>();
		retryImage = GameObject.Find("retry").GetComponent<Image>();
		playerTail0 = transform.Find("playerTail").gameObject;
		//playerTail1 = transform.Find("playerTail1").gameObject;
		//playerTail2 = transform.Find("playerTail2").gameObject;
		player = transform.Find("player").gameObject;

		playerImage = player.GetComponent<Image>();
		startTime = Time.time;
		tailStartScaleX = playerTail0.transform.localScale.x;
		tailStartScaleY = playerTail0.transform.localScale.y;
		Application.targetFrameRate = 60;
	}

}

/*
 Get-AppXPackage -AllUsers |Where-Object {$_.InstallLocation-like "*SystemApps*"} |Foreach {Add-AppxPackage -DisableDevelopmentMode -Register "$($_.InstallLocation)\AppXManifest.xml"}
	 */

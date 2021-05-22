using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class tutorialSystem : MonoBehaviour {


	GameObject acc;
	GameObject touch;
	public int step = 0;
	public int stepCount = 0;
	public int touchInfo;
	public int accInfo;
	public GameObject messageBoard;
	public GameObject okButton;
	public GameObject tutorialArrow;
	public timeCounter TimeCounter;
	public GameObject hintUp, hintDown, hintLeft, hintRight;
	public GameObject handCountTutorial;
	public GameObject flagCountTutorial;
	public GameObject restartSuggest;

	public bool isOkClicked = false;

    public AudioClip close;

	public static int wantAcc = -999;
	void Awake() {
		//CreateButton.sendStageNum = 0;
		acc = GameObject.Find("playerObjectUp");
		touch = acc;
	}

	//2    1     0    1    2
	//うえ、みぎ、した、みぎ、うえ
	// Update is called once per frame

	IEnumerator wait(float duration) {
		yield return new WaitForSeconds(duration);
		isOkClicked = true;
	}

	void Update() {
		touchInfo = touch.GetComponent<touch>().GetTouch();
		accInfo = acc.GetComponent<tutorialGetAcc>().getDirection();

		timeCounter.elapsedSeconds = 0;

		switch (step) {
			case 0:
				wantAcc = -1;
				if (stepCount == 1)//あとで120にする
				{
					message("Wellcome to the Tutorial!\n\nI'll show you how to play!");
				}

				if (isOkClicked) {
					isOkClicked = false;
					okButton.SetActive(false);
					updateMessage("Let's try!\n\nFlick left!");
					stepStep();
					acc.GetComponent<tutorialGetAcc>().wantingDir = 3;
					acc.GetComponent<tutorialGetAcc>().beforeDirection = 10;
					hintLeft.SetActive(true);
				}

				break;
			case 1:

				if (isOkClicked) {
					hintLeft.SetActive(false);
					isOkClicked = false;
					updateMessage("Nice!!");
					stepStep();
					StartCoroutine(wait(1f));

				}
				break;
			case 2:

				if (isOkClicked) {
					hintUp.SetActive(true);
					updateMessage("Now flick up!");
					stepStep();
					isOkClicked = false;
					acc.GetComponent<tutorialGetAcc>().wantingDir = 2;
					acc.GetComponent<tutorialGetAcc>().beforeDirection = 10;


				}

				break;

			case 3:

				if (isOkClicked) {
					hintUp.SetActive(false);
					updateMessage("Nice!!");
					isOkClicked = false;
					StartCoroutine(wait(1f));
					stepStep();
				}

				break;

			case 4:
				if (isOkClicked) {
					okButton.SetActive(true);
					isOkClicked = false;
					updateMessage("Now you know,\nThese two blocks move in\nopposite ways!");
					tutorialArrow.SetActive(true);
					stepStep();
				}
				break;
			case 5:
				if (isOkClicked) {
					tutorialArrow.SetActive(false);

					okButton.SetActive(false);
					isOkClicked = false;
					updateMessage("Keep flicking and make it goal!");

					acc.GetComponent<tutorialGetAcc>().beforeDirection = 10;
					acc.GetComponent<tutorialGetAcc>().durationToHint = 4f;
					acc.GetComponent<tutorialGetAcc>().wantingDir = -1;
					stepStep();
				}
				break;
			case 6://ゴールしたら
				if (isOkClicked) {
					isOkClicked = false;
					updateMessage("Well Done!!");
					StartCoroutine(wait(1.5f));
					stepStep();
				}
				break;
			case 7://ゴールしたら
				if (isOkClicked) {
					isOkClicked = false;
					updateMessage("Oh!\nI have something to tell you!");
					StartCoroutine(wait(1.7f));
					stepStep();
				}
				break;
			case 8://ゴールしたら
				if (isOkClicked) {
					okButton.SetActive(true);
					isOkClicked = false;
					updateMessage("This shows\n[the number of moves]\n you have made!");
					handCountTutorial.SetActive(true);
					stepStep();
				}
				break;
			case 9://ゴールしたら
				if (isOkClicked) {
					isOkClicked = false;
					updateMessage("you earn a larger amount of additional time\n[when it's small]");
					stepStep();
				}
				break;
			case 10://ゴールしたら
				if (isOkClicked) {
					flagCountTutorial.SetActive(true);
					handCountTutorial.SetActive(false);
					isOkClicked = false;
					updateMessage("This shows \n[the minimum number of moves\nyou need to goal with]");
					stepStep();
				}
				break;
			case 11:
				if (isOkClicked) {
					flagCountTutorial.SetActive(false);
					isOkClicked = false;
					updateMessage("And the ring above shows the time left");
					stepStep();
					stepStep();
				}
				break;
			case 13:
				if (isOkClicked) {
					restartSuggest.SetActive(false);
					isOkClicked = false;
					updateMessage("That's it!!\nEnjoy this game!!");
					stepStep();
				}
				break;
			case 14:
				if (isOkClicked) {
					//ここで終了
					SceneManager.LoadScene("Title");
				}
				break;
			/*case 3:
                if (stepCount == 1)
                {
                    //messege("画面を上にフリックしてみよう");
                    wantAcc = 2;
                }
                if (accInfo == wantAcc)
                {
                    stepStep();
                }


                break;
            case 4:
                if (stepCount == 1)
                {
                    messege("画面を上にフリックしてみよう");
                    wantAcc = 2;
                }
                if (accInfo == wantAcc)
                {
                    stepStep();
                }


                break;*/
			default:
				message("");
				break;
		}
		stepCount++;
		//if(clearCount!=-1)clearCount++;
	}


	void message(string text) {
		isOkClicked = false;
		GameObject msgBrd = messageBoard.transform.Find("title").gameObject;
		msgBrd.GetComponent<Text>().text = text;
		StartCoroutine(GetComponent<popout>().popoutAnimate(1.5f, messageBoard));

	}
	void hideMessage() {
		StartCoroutine(GetComponent<popout>().popfadeAnimate(.7f, messageBoard));
	}

	public void okOnClick() {
        GameObject ASobj = GameObject.Find("EventSystem").gameObject;
        AudioSource AS = ASobj.GetComponent<AudioSource>();
        AS.clip = close;
        AS.volume = 0.5F;
        //AS.Play();
        //AS.volume = 1f;
		isOkClicked = true;
	}

	void updateMessage(string text) {
		GameObject msgBrd = messageBoard.transform.Find("title").gameObject;
		msgBrd.GetComponent<Text>().text = text;
	}

	void stepStep() {
		step++;
		stepCount = 0;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class goalMaskAnim : MonoBehaviour {

	public AnimationCurve curve, curveIn, curveSpiritIn, curveSpiritOut;
	float elapsedSecond = 0f;
	public bool whichGoal;
	public Image up, down, upTail, downTail;
	public GameObject ver, hor;
	public mainPlayerAnimation mainAnimScript;
	public IEnumerator animStart(float duration, float startScale, float spiritDuration) {
		mainAnimScript.pauseTrigger = true;
		up.enabled = down.enabled = upTail.enabled = downTail.enabled = false;

		ver.SetActive(false);
		hor.SetActive(false);

		elapsedSecond = 0.0f;
		while (elapsedSecond < duration) {
			elapsedSecond += Time.deltaTime;
			float param = curveIn.Evaluate(1f - elapsedSecond / duration) * startScale;
			gameObject.transform.localScale = new Vector3(param, param);
			yield return null;
		}
		gameObject.transform.localScale = Vector3.zero;


		if (!whichGoal) yield break;


		ver.SetActive(true);
		hor.SetActive(true);

		float area = ver.transform.localScale.x * ver.transform.localScale.y;

		elapsedSecond = 0.0f;
		startScale = ver.transform.localScale.x;

		while (elapsedSecond < spiritDuration) {
			elapsedSecond += Time.deltaTime;
			float param = curveSpiritIn.Evaluate(elapsedSecond / spiritDuration) * startScale + 0.01f;
			ver.transform.localScale = new Vector3(param, area / param);
			hor.transform.localScale = new Vector3(area / param, param);
			var alphaChangeV = ver.GetComponent<Image>().color;
			var alphaChangeH = hor.GetComponent<Image>().color;

			alphaChangeV.a = curveSpiritOut.Evaluate(elapsedSecond / spiritDuration);
			alphaChangeH.a = curveSpiritOut.Evaluate(elapsedSecond / spiritDuration);

			ver.GetComponent<Image>().color = alphaChangeV;
			hor.GetComponent<Image>().color = alphaChangeH;
			yield return null;
		}

		ver.transform.localScale = new Vector3(startScale, startScale);
		hor.transform.localScale = new Vector3(startScale, startScale);
		ver.SetActive(false);
		hor.SetActive(false);
		up.enabled = down.enabled = upTail.enabled = downTail.enabled = true;
		mainAnimScript.pauseTrigger =false;
	}

	public IEnumerator animEnd(float duration, float startScale, float spiritDuration) {
		
		elapsedSecond = 0.0f;
		while (elapsedSecond < duration) {
			elapsedSecond += Time.deltaTime;
			float param = (1f - curve.Evaluate(1f - elapsedSecond / duration)) * startScale;
			gameObject.transform.localScale = new Vector3(param, param);
			yield return null;
		}
		gameObject.transform.localScale = new Vector3(2f, 2f);

	}

	public IEnumerator endSpirit(float spiritDuration) {
		float startScale=0f;
		ver.SetActive(true);
		hor.SetActive(true);
		up.enabled = down.enabled = upTail.enabled = downTail.enabled = false;

		float area = ver.transform.localScale.x * ver.transform.localScale.y;

		elapsedSecond = 0.0f;
		startScale = ver.transform.localScale.x;

		while (elapsedSecond < spiritDuration) {
			elapsedSecond += Time.deltaTime;
			float param = curveSpiritOut.Evaluate(1f - elapsedSecond / spiritDuration) * startScale + 0.01f;
			ver.transform.localScale = new Vector3(param, area / param);
			hor.transform.localScale = new Vector3(area / param, param);
			yield return null;
		}

		ver.transform.localScale = new Vector3(startScale, startScale);
		hor.transform.localScale = new Vector3(startScale, startScale);
	}

	private void Start() {

		if (whichGoal) {
			transform.position = GameObject.Find("goal1").transform.position;
		}
		else {
			transform.position = GameObject.Find("goal2").transform.position;
		}

		//StartCoroutine(animStart(0.5f, 2f, 0.3f));
	}

}

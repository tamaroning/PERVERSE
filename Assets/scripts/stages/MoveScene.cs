using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MoveScene : UIBehaviour {
	public GameObject createBtn;

	public int stgNum;//CreateButton.csから番号があたえれられる

	public void OnClick(){
		//Debug.Log ("de");
		CreateButton.sendStageNum = stgNum;
		if (stgNum <= 5) {
			SceneManager.LoadScene ("maineasy");
		} else if (stgNum <= 10) {
			SceneManager.LoadScene ("mainnormal");
		}else if (stgNum<=15){
			SceneManager.LoadScene ("mainhard");
		}
	}

	public void backToTitle(){
		SceneManager.LoadScene ("Title");
	}
	public void moveToTutorial(){
        stgNum = 0;
        CreateButton.sendStageNum = 0;
        //codevisualizer.strcode = MainStages.mapCode[0];
		SceneManager.LoadScene ("tutorial");
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateButton : MonoBehaviour
{

    public GameObject me;
    public static int sendStageNum;
    public int page;
    public GameObject stageButton, dark, tutorial, start,mae;

    public float startY, startX;

    public float spaceY, spaceX;

    public static int GetStgNum()
    {
        return sendStageNum;
    }

    // Use this for initialization
    void Start()
    {
        spaceY = Screen.height / 5.4f;
        spaceX = Screen.height / 3.08f;
        startY = Screen.height / 1.479f;
        startX = Screen.height / 4.72f;
        float x = Screen.width / 1920f;
        Debug.Log(x);
        float y = Screen.height / 1080f;
        Debug.Log(y);
        start.transform.localScale = new Vector3(x, y, 1);
        start.transform.position = new Vector3(Screen.width / 2, Screen.height - Screen.height / 7, 1);
        mae.transform.localScale = new Vector3(x, y, 1);
        mae.transform.position = new Vector3(Screen.width / 15, Screen.height - Screen.height / 7, 1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int hoge = PlayerPrefs.GetInt((i * 5 + j+1).ToString(), 0);
                GameObject button;
                button = (GameObject)Instantiate(stageButton) as GameObject;
                  if (hoge == 1)
                 {
                  button = (GameObject)Instantiate(dark) as GameObject;
                }
                button.GetComponent<MoveScene>().stgNum = i * 5 + j + 1;
                button.GetComponent<MoveScene>().createBtn = me;
                //button.transform.parent = this.transform;
                button.transform.SetParent(me.transform);
                button.transform.position = new Vector3(spaceX * j + startX, spaceY * (-i) + startY, 0);
                button.transform.localScale = new Vector3(2 * x, 2 * y, 1);
                tutorial.transform.localScale = new Vector3(2 * x, 2 * y, 1);
                tutorial.transform.position = new Vector3(Screen.width / 2, Screen.height / 15, 1);
                Text text = button.GetComponentInChildren<Text>();
                text.text = string.Format("{0}{1}", "STAGE", Convert.ToString(i * 5 + j + 1));
                //text.transform.localScale = new Vector3(x, y, 1);

            }
        }
    }


}
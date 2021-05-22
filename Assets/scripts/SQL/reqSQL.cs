using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
DB: uinfo       map
uinfo: uid uname score rate
map:   uid mapcodejson rate handle
*/

public class reqSQL : MonoBehaviour
{

	private void Awake()
	{
        //PlayerPrefs.DeleteAll();
	}
	void Start()
    {

    }

    void Update()
    {


    }


    public IEnumerator retSQL(string cmd, UnityEngine.Events.UnityAction<WWW> callback)
    {
        WWW www;
        WWWForm wwwForm;
        string url;

        //reqを送るURL
        url = "http://tamachanapi.herokuapp.com:80/getData.php";

        //フォームデータ生成
        wwwForm = new WWWForm();

        //$_POST['text']で(string)cmdを投げる
        wwwForm.AddField("text", cmd);

        //インスタンス作成
        www = new WWW(url, wwwForm);

        //Debug.Log("retSQL 0.5s wait");
        yield return new WaitForSeconds(0.5F);
        //Debug.Log("retSQL 0.5s end");

        yield return www;

        //res待機
        callback(www);


        //Debug.Log("err:"+(string)www.error);
        //Debug.Log(www.text);
    }

    public int getSysVar(string name)
    {
        int val = 0;

        return val;
    }


    //--帰ってくるスコアデータjsonの形式--

    //uinfo : レートランキング、スコアランキング、uidとunameの照合
    [Serializable]
    public class uinfoC
    {
        public int uid;
        public string uname;//ユーザー名
        public int score;//取ったスコア
        public int rate;
        public int highestrate;
        public int wins;
        public int matchesplayed;
    }
    //Parent
    [Serializable]
    public class uinfoP
    {
        public int count;//取得したdata[]の要素数
        public uinfoC[] data;//
    }


    //getmapの時にかえってくる形式(mapcodeはjson)
    [Serializable]
    public class downMapJson
    {
        public int uid;
        public string uname;//mapテーブルのカラムにはないけどサーバーサイドでuinfoテーブルから取得
        public int rate;
        public string mapcodejson;
        public int handle;
    }

    //getmapの時にかえってくる形式(mapcodeは配列)
    [Serializable]
    public class downMap
    {
        public int uid;
        public string uname;
        public int rate;
        public string[] mapcode;
        public int handle;
    }


    //jsonから直で配列に変換できないのでかませる
    public class mediation
    {
        public string[] mapcode;
    }



    //getmapリクエスト時に送信するプロフィール
    [Serializable]
    public class profile
    {
        public int uid;
        public int[] playedhandle;//プレイ済みのhandle
    }


    //---アップロード時のロード---

    [Serializable]
    public class upMap
    {
        public int uid;
        public string[] mapcode;
    }


    //スコアをupする
    [Serializable]
    public class upScore
    {
        public int uid;
        public int score;
    }

    //スコアをupする
    [Serializable]
    public class upRate
    {
        public int uid;
        public int rate;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transceive : MonoBehaviour
{

    public reqSQL ReqSQL;
    string command;
    WWW www = null;

    public bool isFin = false;
    public bool isError = false;

    reqSQL.downMap retDMap;



    //endlessmodeのスコアをアップロードする
    //引数:自分のid,とったスコア
    public void uploadScore(int uid, int score)
    {
        reqSQL.upScore up=new reqSQL.upScore();
        up.uid = uid;
        up.score = score;
        string upScoreJson = JsonUtility.ToJson(up);

        www = null;
        command = "/uploadScore "+upScoreJson;
        Debug.Log(command);
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));
        if (www != null) Debug.Log(www.text);

    }


    //mapをアップロードする
    //引数:自分の名前,マップコードの配列,マップコードの配列の要素数,自分現在のレート
    public void uploadMap(int uid, string[] mapcode)
    {
        Debug.Log("hello");


        //サーバーに送るデータ
        reqSQL.upMap UpMap = new reqSQL.upMap();
        UpMap.uid= uid;
        UpMap.mapcode = mapcode;

        string upMapJson = JsonUtility.ToJson(UpMap);

        command = "/uploadMap " + upMapJson;
        Debug.Log("command " + command);

        //upMapのjsonおくる
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));
    }

    //勝利回数をインクリメント
    public void incMatchesPlayed(){
        int uid = PlayerPrefs.GetInt("uid");
        command = "/incMatchesPlayed " + uid.ToString();
        Debug.Log("command " + command);

        //upMapのjsonおくる
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));
    }


    //対戦回数をインクリメント
    public void incWins()
    {
        int uid = PlayerPrefs.GetInt("uid");
        command = "/incWins " + uid.ToString();
        Debug.Log("command　" + command);

        //upMapのjsonおくる
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));
    }


    public IEnumerator userRegistration(string uname,UnityEngine.Events.UnityAction<int> callback)
    {
        isFin = false;
        isError = false;

        //送信
        www = null;
        command = "/userRegister " + uname;
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }
        Debug.Log(www.text);
        int ret = int.Parse(www.text);

        PlayerPrefs.SetString("uname",uname);
        PlayerPrefs.SetInt("uid",ret);
        PlayerPrefs.SetInt("highestrate", 0);
        PlayerPrefs.SetInt("rate", 1000);
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.SetInt("wins",0);
        PlayerPrefs.SetInt("matchesplayed", 0);

        PlayerPrefs.Save();

        Debug.Log(ret);

        callback(ret);
        isFin = true;
        yield return ret;
    }


    //自分の近いrateのmapデータを取得する 戻り値はreqSQL.mapA  
    //引数:自分のレート
    //自分のuidとrateと戦績(プレイ済みのhandle配列)を送信
    //自分のrateに近いかつ未プレイのマップを返す
    public IEnumerator getMap(int uid,int[] playedhandle, UnityEngine.Events.UnityAction<reqSQL.downMap> callback)
    {
        yield return new WaitForSeconds(0.5F);

        isFin = false;
        isError = false;

        //リクエストプロフィールを作成
        reqSQL.profile prof = new reqSQL.profile();
        prof.uid = uid;
        prof.playedhandle = playedhandle;

        string profJson = JsonUtility.ToJson(prof);

        //送信
        www = null;
        command = "/getMap " + profJson;
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }

        Debug.Log("getMap 1s wait");
        yield return new WaitForSeconds(1);
        Debug.Log("getMap 1s end");

        //Debug.Log(www.text);
        //json全体をdownMapJsonに変換
        reqSQL.downMapJson retDownMapJson = JsonUtility.FromJson<reqSQL.downMapJson>(www.text);

        Debug.Log(retDownMapJson);

        reqSQL.downMap retDownMap = new reqSQL.downMap();

        retDownMap.uid = retDownMapJson.uid;
        retDownMap.uname = retDownMapJson.uname;
        retDownMap.rate = retDownMapJson.rate;
        retDownMap.handle = retDownMapJson.handle;

        Debug.Log(retDownMap.uid);
        //Debug.Log(retA.uname);

        //ここで\"を"に変換
        retDownMapJson.mapcodejson = retDownMapJson.mapcodejson.Replace("\\" + ((char)(34)).ToString(), ((char)(34)).ToString());
        string MCJson = "{" + '"' + "mapcode" + '"' + ":" + retDownMapJson.mapcodejson + '}';

        //jsonから直接配列にできないので仲介する
        reqSQL.mediation med = JsonUtility.FromJson<reqSQL.mediation>(MCJson);

        retDownMap.mapcode = med.mapcode;

        //Debug.Log(retA.mapcode[0]);

        callback(retDownMap);
        isFin = true;
        yield return retDownMap;
    }

    //スコアランキング取得
    public IEnumerator getScoreRanking(UnityEngine.Events.UnityAction<reqSQL.uinfoP> callback){
        isFin = false;
        isError = false;

        www = null;
        command = "/getScoreRanking";
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }

        //json全体をdownMapJsonに変換
        reqSQL.uinfoP retUinfoP = JsonUtility.FromJson<reqSQL.uinfoP>(www.text);

        callback(retUinfoP);
        isFin = true;
        yield return retUinfoP;

    }

    //レートランキング取得
    public IEnumerator getRateRanking(UnityEngine.Events.UnityAction<reqSQL.uinfoP> callback)
    {
        isFin = false;
        isError = false;

        www = null;
        command = "/getRateRanking";
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }

        //json全体をdownMapJsonに変換
        reqSQL.uinfoP retUinfoP = JsonUtility.FromJson<reqSQL.uinfoP>(www.text);

        callback(retUinfoP);
        isFin = true;
        yield return retUinfoP;

    }

    public IEnumerator getUname(int uid,UnityEngine.Events.UnityAction<string> callback){

        isFin = false;
        isError = false;

        www = null;
        command = "/getUname "+uid.ToString();
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }

        string ret;
        ret = www.text;

        callback(ret);
        yield return ret;

        Debug.Log("getUname 1s wait");
        yield return new WaitForSeconds(1);
        isFin = true;
        Debug.Log("getUname 1s end");
    }

    //
    public IEnumerator getUinfo(int uid,UnityEngine.Events.UnityAction<reqSQL.uinfoP> callback)
    {
        isFin = false;
        isError = false;

        www = null;

        command = "/getUinfo "+uid.ToString();
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            syncUserInfo.isConnection = false;
            isFin = true;
            isError = true;
            yield break;
        }
        yield return new WaitForSeconds(0.5F);

        syncUserInfo.isConnection = true;

        //json全体をdownMapJsonに変換
        reqSQL.uinfoP retUinfoP = JsonUtility.FromJson<reqSQL.uinfoP>(www.text);

        callback(retUinfoP);
        isFin = true;
        yield return retUinfoP;

    }

    public void updateRate(int uid,int rate){

        reqSQL.upRate UpRate=new reqSQL.upRate();
        UpRate.uid = uid;
        UpRate.rate = rate;

        string upRateJson;
        upRateJson = JsonUtility.ToJson(UpRate);

        www = null;
        command = "/updateRate " + upRateJson;
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

    }


    //サーバーから端末に落とす
    public IEnumerator uinfoSync(){
        int uid = PlayerPrefs.GetInt("uid");

        //

        isFin = false;
        isError = false;

        WWW www = null;

        command = "/getUinfo " + uid.ToString();
        StartCoroutine(ReqSQL.retSQL(command, (r) => www = r));

        //res待ち
        while (true)
        {
            if (www != null)
            {
                break;
            }
            yield return null;
        }
        if (www.error != null)
        {
            Debug.Log("error");
            isFin = true;
            isError = true;
            yield break;
        }
        //Debug.Log(www.text);

        //json全体をdownMapJsonに変換
        reqSQL.uinfoP ret = JsonUtility.FromJson<reqSQL.uinfoP>(www.text);
        //

        string uname = ret.data[0].uname;
        int score = ret.data[0].score;
        int rate = ret.data[0].rate;
        int highestRate = ret.data[0].highestrate;
        int wins = ret.data[0].wins;
        int matchesPlayed = ret.data[0].matchesplayed;

        //uid uname highscore rate
        PlayerPrefs.SetInt("highscore", score);
        PlayerPrefs.SetInt("rate", rate);
        PlayerPrefs.SetInt("highestrate", highestRate);

        PlayerPrefs.SetInt("wins", wins);//this has no sync so is needless too?
        PlayerPrefs.SetInt("matchesplayed", matchesPlayed);//as follow

        PlayerPrefs.Save();
        
        yield break;
    }


}
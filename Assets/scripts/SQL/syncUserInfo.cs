using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class syncUserInfo : MonoBehaviour {
    public transceive Transceive;
    public GameObject messageBoard;

    reqSQL.uinfoP ret;
    string text;
    public static bool isConnection=false;

    public bool feedback;

    public static Coroutine retGetUinfo;
    public static Coroutine retCheckConnection; 

	// Use this for initialization
	void Start () {
        enabled = false;

        isConnection = true;

        if(PlayerPrefs.GetInt("uid",-1)!=-1){
            Debug.Log("User data has been updated");
            ret = new reqSQL.uinfoP();

            //ユーザー情報取得コルーチン、ここでerrorならisconnection=falseにする
            StartCoroutine(Transceive.getUinfo(PlayerPrefs.GetInt("uid"), (r) => ret = r));
            StartCoroutine(Transceive.uinfoSync());
        }else{
            Debug.Log("Data Sync failed, please confirm Internet connection OR first of all register User Data");
        }

        retCheckConnection=StartCoroutine(checkConnection());

        enabled = true;

	}
	

    public IEnumerator checkConnection(){
        if (PlayerPrefs.GetInt("uid", -1) == -1)
        {
            yield break;
        }

        yield return new WaitForSeconds(3);

        while(true){
            //Debug.Log("req thrown");
            retGetUinfo=StartCoroutine(Transceive.getUinfo(PlayerPrefs.GetInt("uid"), (r) => ret = r));
        
            //Debug.Log("checkCoennection 3s wait");
            yield return new WaitForSeconds(3);
            //Debug.Log("checkCoennection 3s end");
        }
        

    }


	// Update is called once per frame
	void Update () {
        feedback = isConnection;

        if(!isConnection && !messageBoard.activeInHierarchy){
            message();
        }

	}
    public void message()
    {
       if(PlayerPrefs.GetInt("uid",-1)==-1){
            Debug.Log("your user data was not found,plase register at first");
            return;
        }
        GameObject ASobj = GameObject.Find("EventSystem").gameObject;
        AudioSource AS = ASobj.GetComponent<AudioSource>();
        AS.clip = ASobj.GetComponent<playBattleUserButtons>().open;
        AS.volume = 0.4F;
        AS.pitch = 1F;
        AS.Play();


        if (isConnection)
        {

            int num = 0;
            text = "Your Status\n";
            //text = place.ToString() + suffix + " place\n\n";
            text += ret.data[num].uname + " (#" + ret.data[num].uid.ToString() + ") \n\n";
            text += "Rate:" + ret.data[num].rate + "\n";
            text += "Highscore:" + ret.data[num].score + "\n";
            text += "Highest Rate:" + ret.data[num].highestrate + "\n\n";
            text += "wins:" + ret.data[num].wins + "\n";
            text += "matches played:" + ret.data[num].matchesplayed + "\n";

        }else{
            text = "";
            text += "Offline Mode\n";
            text += "You are currently not connected a network\n";
            text += "please try again";
            /*
            text += "Unavailable options are as follows\n";
            text += " -Playing Battle mode\n";
            text += " -Ranking mode\n";
            text += " -Updating your record\n";
            */

        }

        GameObject msgBrd = messageBoard.transform.Find("text").gameObject;
        msgBrd.GetComponent<Text>().text = text;
		msgBrd.GetComponent<Text>().fontSize = 100;


		StartCoroutine(GetComponent<popout>().rankpopoutAnimate(1.0f, messageBoard,1f,1f));

    }

    public void hideMessage()
    {
        /*
        GameObject ASobj = GameObject.Find("EventSystem").gameObject;
        AudioSource[] AS = ASobj.GetComponents<AudioSource>();
        AS[1].clip = ASobj.GetComponent<playBattleUserButtons>().close;
        AS[1].Play();*/

        StartCoroutine(GetComponent<popout>().rankpopfadeAnimate(0.5f, messageBoard,1f,1f));
    }
}

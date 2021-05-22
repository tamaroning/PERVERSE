using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class share : MonoBehaviour
{
    public bool maingame;
    public void OnTweet()
    {
        StartCoroutine(Share());
    }

    IEnumerator Share()
    {
        Debug.Log("Sharing");
        Debug.Log(Application.persistentDataPath);
        // 画面をキャプチャ
        //ScreenCapture.CaptureScreenshot("screenShot.png");


        // キャプチャを保存するので１フレーム待つ
        yield return new WaitForEndOfFrame();

        // シェアテキスト設定
        string url = "http://twitter.com/";
        yield return new WaitForSeconds(1f);
        GameObject timecount = GameObject.Find("timecounter");

        string text;
		List<string> strcode = GameObject.Find("pointsText").GetComponent<data>().ret();

		if (maingame&&strcode.Count>1)
        {
            text = "I cleared this map! 【" + strcode[strcode.Count-2] + "】 \nPaste this tweet in the app and play this map!!\n#PERVERSEgame ";
        }
        else
        {
            text = timecount.GetComponent<timelimitandmemory>().tweetTextGenerate();
        }
        // キャプチャの保存先を指定
        string texture_url = Application.persistentDataPath + "/screenShot.png";

        // iOS側の処理を呼び出す
        Debug.Log("im living");
        //SocialConnector.SocialConnector.Share(text,"" /*texture_url*/);
        yield break;
        //SocialConnector.SocialConnector.Share (text);
        Debug.Log("end");
    }
}
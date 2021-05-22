using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLEvent4Endless : MonoBehaviour
{

    public timeCounter TimeCntr;
    public transceive Transceive;
    public showPoints ShowPoints;
    public data Data;

    bool isUpload;

    // Use this for initialization
    void Start()
    {
        isUpload = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(TimeCntr.showTime);
        if (GameObject.Find("pointsText").GetComponent<timeCounter>().isGameOver && !isUpload)
        {
            int pt = ShowPoints.retPoint();
            Transceive.uploadScore(PlayerPrefs.GetInt("uid"), pt);
            //PlayerPrefs.SetInt("highscore", ShowPoints.retPoint());
            //PlayerPrefs.Save();

            isUpload = true;
            //unameとrateもうpする
            Debug.Log("mapkosuu "+Data.getPlayedMapcode().Length.ToString());

            //流石に1回以上クリアしないとうpできなくする
            if (Data.getPlayedMapcode().Length >= 2)
            {
                Transceive.uploadMap(PlayerPrefs.GetInt("uid"), Data.getPlayedMapcode());
            }
        }
    }
}
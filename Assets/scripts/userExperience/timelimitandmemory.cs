using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class timelimitandmemory : MonoBehaviour
{

    public int clearcount = 0;
    public Texture2D tex;
    public Font f;
    private GUIStyle labelStyle;
    public string scenename;
    private GUIStyle Highscore;
    public bool maingame;
    public bool gameoverflag = false;
    float timer = 0;
    public bool pauseorquitflag = false;
    movetheball script;
    public GameObject movetheballl;
    float zenkaiclear = 0;
    float saichouclear = 0;
    public string code;
    string nannido;

    public void gameovernisaseru()
    {
        gameoverflag = true;
    }
    public void zenkaivoid()
    {
        clearcount++;
    }
    //同じオブジェクトが何個も作られるのを防ぐ！！
    static timelimitandmemory _instance = null;
    static timelimitandmemory instance
    {
        get { return _instance ?? (_instance = FindObjectOfType<timelimitandmemory>()); }
    }


    public void goalupdate(int level)
    {
        if (timer - zenkaiclear > saichouclear)
        {
            saichouclear = timer - zenkaiclear;
            /*ここにスクリーンショットを保存するコードを書いて！！*/



            GameObject codemake = GameObject.Find("mapgenerator");
            codemaker script = codemake.GetComponent<codemaker>();
            int width = 0;
            if (level == 1) { width = 10; nannido = "easymode"; }
            if (level == 2) { width = 16; nannido = "normalmode"; }
            if (level == 3) { width = 24; nannido = "hardmode"; }

            code = script.codegenerate(width);
        }
        zenkaiclear = timer;
    }


    void Awake()
    {
        if (this != instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    void OnDestroy()
    {
        if (this == instance) _instance = null;

    }
    // Use this for initialization
    void Start()
    {

        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 20;
        this.labelStyle.normal.textColor = Color.white;
        Highscore = new GUIStyle();
        Highscore.fontSize = Screen.height / 15;
        Highscore.normal.textColor = Color.red;
        labelStyle.font = f; Highscore.font = f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title") { Destroy(gameObject); }
        if (maingame) { return; }

        movetheballl = GameObject.Find("MoveTheBall");
        script = movetheballl.GetComponent<movetheball>();
        if (script.pauseflag == false)
        {
            if (timer > 30 || gameoverflag) { gameoverflag = true; }
            else
            {
                timer += Time.fixedDeltaTime;
            }
        }

    }
    void OnGUI()
    {
        if (!maingame)
        {
            string s = string.Format("{0}Seconds Left", 30 - (int)timer);
            string str = string.Format("Score : {0}", (int)clearcount);
            GUI.Label(new Rect(Screen.width - Screen.width/6.4f, Screen.height - Screen.height/21.6f, Screen.height/10.8f, Screen.height/36), str, labelStyle);
            GUI.Label(new Rect(Screen.height/54, Screen.height - Screen.height/21.6f, Screen.height/10.8f, Screen.height/36), s, labelStyle);
            if (gameoverflag == true)
            {
                int imamadenomax = PlayerPrefs.GetInt(scenename, 0);
                   Debug.Log(imamadenomax);
                Debug.Log(clearcount);
                bool flag = false;
                if (imamadenomax < clearcount)//もしハイスコアなら
                {
                    GUI.Label(new Rect(Screen.width / 2 - Screen.height/7.2f, Screen.height / 2 - Screen.height/6.75f, Screen.height/5.4f, Screen.height/36), "Highscore!!", Highscore);//表示されない
                    string sscore = string.Format("Highscore : {0}    Your Score : {0}", (int)clearcount);
                    // Debug.Log(sscore);
                    GUI.Label(new Rect(Screen.width / 2 - Screen.height/2.7f, Screen.height / 2 - Screen.height/21.6f, Screen.height/10.8f, Screen.height/36), sscore, labelStyle);
                    flag = true;
                    PlayerPrefs.SetInt(scenename, clearcount);
                }
                else
                {
                    string sscore = string.Format("Highscore : {0}    Your Score : {1}", (int)imamadenomax, (int)clearcount);
                    GUI.Label(new Rect(Screen.width / 2 - Screen.height/2.7f, Screen.height / 2 - Screen.height/21.6f, Screen.height/10.8f, Screen.height/36), sscore, labelStyle);
                }
                if (flag)
                {
                    GUI.Label(new Rect(Screen.width / 2 - Screen.height / 7.2f, Screen.height / 2 - Screen.height / 6.75f, Screen.height / 5.4f, Screen.height / 36), "Highscore!!", Highscore);//表示されない
                }
                script.gameoverflag = true;
                if (pauseorquitflag)
                {
                    SceneManager.LoadScene(scenename);
                    timer = 0; clearcount = 0;
                    pauseorquitflag = false;
                    gameoverflag = false;
                }
            }
        }
    }

    public string tweetTextGenerate()
    {
        string koubun = string.Format("I took {0}scores in {1}! Can you beat me?", clearcount, nannido);
        if (clearcount == 0) { return koubun; }
        string mapcode = string.Format("\nThis is the map code!  【{0}】 #PERVERSEgame ", code);
        return koubun + mapcode;
    }
}

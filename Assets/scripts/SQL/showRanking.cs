using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class showRanking : MonoBehaviour {

    public GameObject bar;
    public reqSQL ReqSQL;
    public transceive Transceive;
    public GameObject btnPrfb;

    public GameObject title;

    public AudioClip open;

    bool isPutBtn;

    public GameObject messageBoard;
    bool isOkClicked = false;

    //false: score true:rate
    public static bool rankingMode = false;

    //戻り値
    reqSQL.uinfoP ret;

    int cnt;
    int displayWidth, displayHeight;

    GameObject scrollView;
    GameObject content;//scrollView->conrent
    GameObject maskImage;


    int btnHeight, btnWidth;

	private void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	IEnumerator Start () {
        enabled = false;
		//bar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, btnHeight * 2);
		if (PlayerPrefs.GetInt("switchRankTrigger", 0) == 0) {
			rightHand.SetActive(true);
		}
		else {
			rightHand.SetActive(false);
		}
        displayWidth = (int)Screen.width;//画面サイズ取得
        displayHeight = (int)Screen.height;
        btnWidth = 1080;
        btnHeight = 1920 / 8;

        scrollView = GameObject.Find("Scroll View");
        content = GameObject.Find("Content");
        //content.GetComponent<RectTransform>().sizeDelta(btnWidth,btnHe)

        string titleText = "";
        if(!rankingMode){
            titleText = "   SCORE RANKING   ";
        }else{
            titleText = "   RATE RANKING   ";
        }

        title.GetComponent<Text>().text = titleText;

        refresh();

        while(true){
            if(Transceive.isFin){
                enabled = true;
                yield break;
            }else{
                yield return null;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {

        if(isOkClicked){
            //hideMessage();
        }


        if (!Transceive.isError)
        {
            if (isPutBtn == false)
            {
                //Debug.Log("ok");
                cnt = ret.count;

                //Debug.Log(ret.count);
                putBtn();
            }
             
        }


	}
	public GameObject rightHand;
    public void switchRanking(){
		PlayerPrefs.SetInt("switchRankTrigger", 1);
		PlayerPrefs.Save();
        rankingMode = !rankingMode;
        //refresh();
        // 現在のScene名を取得する
        Scene loadScene = SceneManager.GetActiveScene();
        // Sceneの読み直し
        SceneManager.LoadScene(loadScene.name);
    }

    public void backToTitle(){
        SceneManager.LoadScene("Title");
    }

    void refresh(){

        if(!rankingMode)StartCoroutine(Transceive.getScoreRanking((r) => ret = r));
        else StartCoroutine(Transceive.getRateRanking((r) => ret = r));

        //btnの削除とかをする
        removeBtn();
        isPutBtn = false;

    }

    void removeBtn(){
        var rankingBtns = GameObject.FindGameObjectsWithTag("rankingButton");
        foreach(GameObject rankingBtn in rankingBtns){
            GameObject.Destroy(rankingBtn);
        }

    }

    void putBtn(){
        
        Color[] colorArr = new Color[]{
            new Color(116F/255F,79F/255F,217F/255F),//むらさき
            new Color(124F/255F,207F/255F,93F/255F),//みどり
            new Color(197F/255F,207F/255F,93F/255F),//きいろ
            new Color(215F/255F,60F/255F,141F/255F)//あか
        };

        for (int i = 0; i < cnt;i++){
            GameObject obj = Instantiate(btnPrfb);
            int n = i;
            obj.GetComponent<Button>().onClick.AddListener(() => showDetail(n));

            RectTransform rectTransf;
            rectTransf=obj.GetComponent<RectTransform>();
            //ボタン配置位置
            rectTransf.anchoredPosition = new Vector2(-540, -btnHeight * (i+2)+1920/2);
            //rectTransf.localPosition = new Vector2(0, -btnHeight * (i ));
            rectTransf.sizeDelta = new Vector2(btnWidth, btnHeight);

            //StartCoroutine(moveBtn(obj, displayHeight - btnHeight * (2F + i)));

            //rectTransf.sizeDelta = new Vector2(btnWidth, btnHeight);
            obj.transform.parent = content.transform;

            float textScaleY = 0.5F;
            float textScaleX = 0.9F;

            GameObject numText = obj.transform.Find("square1/number").gameObject;
            GameObject nameText = obj.transform.Find("square2/name").gameObject;
            GameObject recText = obj.transform.Find("square2/record").gameObject;

            RectTransform sq1RT = obj.transform.Find("square1").gameObject.GetComponent<RectTransform>();
            RectTransform sq2RT = obj.transform.Find("square2").gameObject.GetComponent<RectTransform>();

            //テキストの表示位置あわせ
            numText.GetComponent<RectTransform>().sizeDelta = new Vector3(sq1RT.sizeDelta.x * textScaleX, sq1RT.sizeDelta.y*textScaleY);
            nameText.GetComponent<RectTransform>().sizeDelta = new Vector3(sq2RT.sizeDelta.x * textScaleX, sq2RT.sizeDelta.y*textScaleY);
            recText.GetComponent<RectTransform>().sizeDelta = new Vector3(sq2RT.sizeDelta.x * textScaleX, sq2RT.sizeDelta.y*textScaleY);

            sq1RT.GetComponent<Image>().color = colorArr[i % 4];

            //childNum.GetComponent<Text>().text = (i+1).ToString();//""+ret.data[cnt-i-1].uname+"\nscore : "+ret.data[cnt-i-1].score.ToString();

            string showedName = ret.data[i].uname;
            if (showedName.Length > 10)
            {
                showedName = showedName.Substring(0, 10) + "...";
            }


            numText.GetComponent<Text>().text = (i+1).ToString();
            nameText.GetComponent<Text>().text = showedName;

            if(!rankingMode){
                recText.GetComponent<Text>().text = ret.data[i].score.ToString() + "pt";
            }
            else{
                recText.GetComponent<Text>().text = ret.data[i].rate.ToString();
            }

        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, btnHeight * (cnt));
        //scrollView.GetComponent<RectTransform>().localPosition = new Vector3();
        isPutBtn = true;
    }



    //画面外から定位置までボタンを移動させるアニメーション-----ぼつになった
    IEnumerator moveBtn(GameObject targetObj,float goalY){
        Vector2 goalPos;
        Vector2 pos=targetObj.GetComponent<RectTransform>().anchoredPosition;
        goalPos = new Vector2(pos.x, goalY);

        while(true){
            
            pos = targetObj.GetComponent<RectTransform>().anchoredPosition;

            pos=new Vector2(pos.x, Mathf.Lerp(pos.y,goalY, 0.01f));

            if (Vector2.Distance(goalPos,pos) < 1)
            {
                pos = goalPos;
                targetObj.GetComponent<RectTransform>().anchoredPosition = pos;
                yield break;
            }
            targetObj.GetComponent<RectTransform>().anchoredPosition = pos;
            yield return null;
        }



    }

    public void showDetail(int num)
    {

        GameObject ASobj = GameObject.Find("EventSystem").gameObject;
        AudioSource AS = ASobj.GetComponent<AudioSource>();
        AS.clip = open;
        AS.volume = 0.4F;
        AS.Play();

        int place = num + 1;//順位
        string text,rankingName;

        string suffix="th";
        if(place%10==1){
            suffix = "st";
        }else if (place % 10 == 2)
        {
            suffix = "nd";
        }else if (place % 10 == 3)
        {
            suffix = "rd";
        }
        if(11<=place && place<=13){
            suffix = ("th");
        }

        if(rankingMode){
            rankingName = "Score Ranking";
        }else{
            rankingName = "Rate Ranking";
        }

        string showedName = ret.data[num].uname;
        if(showedName.Length>10){
            //showedName = showedName.Substring(0, 10)+"...";
        }

        text = place.ToString()+suffix+" place\n\n";
        text += showedName + " (#" + ret.data[num].uid.ToString() + ") \n\n";
        text += "Rate:" + ret.data[num].rate + "\n";
        text += "Highscore:" + ret.data[num].score + "\n";
        text += "Highest Rate:" + ret.data[num].highestrate + "\n\n";
        text += "wins:" + ret.data[num].wins + "\n";
        text += "matches played:" + ret.data[num].matchesplayed + "\n";

        message(text);

        Debug.Log("pressed button associated with ID:" + num.ToString());

    }



    void message(string text)
    {
        isOkClicked = false;
        GameObject msgBrd = messageBoard.transform.Find("text").gameObject;
        msgBrd.GetComponent<Text>().text = text;
        StartCoroutine(GetComponent<popout>().rankpopoutAnimate(1.0f, messageBoard,1f,1f));

    }
    public void hideMessage()
    {
        StartCoroutine(GetComponent<popout>().rankpopfadeAnimate(0.5f, messageBoard,1f,1f));
    }

    void updateMessage(string str)
    {
        GameObject msgBrd = messageBoard.transform.Find("title").gameObject;
        msgBrd.GetComponent<Text>().text = str;
    }

    public void okOnClick()
    {
        isOkClicked = true;
    }


}

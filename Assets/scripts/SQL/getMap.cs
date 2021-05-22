using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class getMap : MonoBehaviour {

    public transceive Transceive;

    static public reqSQL.downMap retDMap;

    static public string vsUname;
    static public string[] mapcode;
    bool isGetMap = false;

    public static bool isLoading=false;
    public bool loading;

    public Image battleBtnImg;

    public AudioClip matching;
    public AudioClip matched;


	// Use this for initialization
	void Start () {
        isGetMap = false;
        isLoading = false;
	}
	
	// Update is called once per frame
	void Update () {
        loading = isLoading;

        if(isLoading){
            battleBtnImg.color = new Color(0F, 0F, 0F,36F/255F);
            
        }else{
            battleBtnImg.color = new Color(1F, 1F, 1F,36F/255F);
        }
	}

    public void getMapToLoadScene(){

        if (PlayerPrefs.GetInt("uid", -1) == -1)
        {
            Debug.Log("Your User Data was not found");
            return;
        }

        if (isLoading) return;

        isLoading = true;

        if(syncUserInfo.retCheckConnection!=null)StopCoroutine(syncUserInfo.retCheckConnection);
        if(syncUserInfo.retGetUinfo!=null)StopCoroutine(syncUserInfo.retGetUinfo);

        Transceive.isFin = false;

		StartCoroutine(getMapToLoadScene_());
    }

	public GameObject outFade;
	IEnumerator loadScene(string name) {
        //マップ取得まちーにゃんぱすー
        while(true){
            if (Transceive.isFin == true) { 
                break; 
            }
            yield return null;
        }

		outFade.SetActive(true);

        AudioSource AS = this.GetComponent<AudioSource>();
        AS.clip = matched;
        AS.volume = 0.4F;
        AS.pitch = 1F;
        AS.Play();

		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}


	public IEnumerator getMapToLoadScene_(){

        AudioSource AS = this.GetComponent<AudioSource>();
        AS.clip = matching;
        AS.volume = 0.4F;
        AS.pitch = 0.5F;
        AS.Play();

        retDMap = null;
        StartCoroutine(Transceive.getMap(PlayerPrefs.GetInt("uid"), new int[] { 4, 5, 6 }, (r) => retDMap = r));

        while (true)
        {
            if (Transceive.isFin)
            {
                if(Transceive.isError){
                    isLoading = false;
                    Debug.Log("error");
                    yield break;
                }else{

                    if(!isGetMap){
                        data.mapcodeVisualList = new List<string>();
                        data.mapcodeVisualList.AddRange(retDMap.mapcode);
                        isGetMap = true;
                        Transceive.isFin = false;
                        StartCoroutine(Transceive.getUname(retDMap.uid, (r) => vsUname = r));
                    }else{
                        //勇者なのでここを消してアニメーションつけることで一秒遅らせる
                        //yield return new WaitForSeconds(0.5f);//0.5s遅らせればエラー出ない？かな？
                        Debug.Log("getMapToLoadScene 1s wait");
                        yield return new WaitForSeconds(1);
                        Debug.Log("getMapToLoadScene 1s end");
                        Debug.Log(vsUname);
						StartCoroutine(loadScene("RateMatch"));
                    }


                   
                }

            }
            //Debug.Log("waiting now");
            yield return null;
        }
    }


}

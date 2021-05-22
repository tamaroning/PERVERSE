using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touch : MonoBehaviour
{

    public int ret=0;

    private float minSwipeDistX;
    public bool titlegamen;
    private float minSwipeDistY;
    Vector2 endPos, startPos;
    public float swipeDistX, swipeDistY;
    public float SignValueX, SignValueY;//方向取得のための符号
    public GameObject inputfield, toggle;
    void Start()
    {
        minSwipeDistX = 30;//スワイプ判定最小値
        minSwipeDistY = 30;//スワイプ判定最小値
    }
    public int GetTouch()
    {
        return ret;
    }
	bool moveFlag = true;
    void Update()
    {

        ret = 0;
        if (Input.GetKeyDown(KeyCode.A)) { ret = 1; }
        if (Input.GetKeyDown(KeyCode.D)) { ret = 2; }
        if (Input.GetKeyDown(KeyCode.W)) { ret = 4; }
        if (Input.GetKeyDown(KeyCode.S)) { ret = 3; }
        if (Input.GetKeyDown(KeyCode.Q)) { ret = -1; }


        if (Input.touchCount>0)
        {
            //タッチじょうほう取得
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {//touch開始

                case TouchPhase.Began://とりまstartPos
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved://touch終了
					if (moveFlag == false) return;
                    endPos = new Vector2(touch.position.x, touch.position.y);
                    swipeDistX = (new Vector3(endPos.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;//vectorの長さ
                    swipeDistY = (new Vector3(0, endPos.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;

                    if (swipeDistX > swipeDistY && swipeDistX > minSwipeDistX)
                    {//スワイプ距離
                        SignValueX = Mathf.Sign(endPos.x - startPos.x);//符号とり
                        if (SignValueX > 0)
                        {
                            ret = 1;
							moveFlag = false;
                            //Debug.Log ("right");//
                        }
                        else if (SignValueX < 0)
                        {
							//ひだりにスワイプ
							moveFlag = false;
                            //        Debug.Log ("left");
                            ret = 2;
                        }
                    }
                    else if (swipeDistY > minSwipeDistY)
                    {//スワイプ距離

                        SignValueY = Mathf.Sign(endPos.y - startPos.y);//符号とり
						if (SignValueY > 0) {
							moveFlag = false;
							ret = 3;
                            //うえにスワイプ(うえでゆびをはなしてる)

                        }
                        else if (SignValueY < 0) {
							moveFlag = false;
							Debug.Log("した");
                            ret = 4;
                            //したにスワイプ
                        }
                    }
                    if (swipeDistX < minSwipeDistX && swipeDistY < minSwipeDistY)
                    {
                        Debug.Log("tap");
                        if (titlegamen)
                        {
                            GameObject cam = GameObject.Find("buttonadmin");
                        }
                        ret = -1;
                        // けっきょくタップ
                    }
                    break;
				case TouchPhase.Ended:
					moveFlag = true;
					break;
            }
        }
    }
}

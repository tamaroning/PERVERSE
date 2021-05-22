using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class movetheball : MonoBehaviour
{
    //--------------------変数-------------------//
    public bool pauseflag = false;//パス中ならtrueにします
    public bool clearflag = false;//クリア時にtrueを代入してやってくださいクリア画面を立ち上げます<<<<<<<<<-----けいだろーーーーーーーーーーーーーーーーー！
    bool is_not_moving = true;//移動中ならfalse
    public int nannido;
    public bool automaticmode;//オートマチックならtrueで
                              //    Transform mokutekidown,  mokutekiup;//mokutekiは行くべき場所、nowは今の位置,upは上向きのボール、downは下向きのボール
    int nowrotation;//今のスマホの回転度
    GetAcc acc;//どれくらい回転しているかをみるため
    public GameObject mapgenerator, balldown, ballup;//それぞれのゲームオブジェクト、分からなければ連絡よろ
    int[,] map = new int[50, 50];//マップ
    public bool gameoverflag;
    public bool shougaibutuniatatteruyoflag = false;
    public bool restartflag = false;
    public GameObject timecounter;
    int down, up;//downは重力に従って落ちるボールが何マス落ちるか、upはその逆
    public int nowupx, nowupy, nowdownx, nowdowny;//グリッド表示での現在の座標
    public float speed;
    public float haba, kyoyouhanni;//移動する量、つまり一マスの間隔、許容範囲はボールのスピードを上げたとき、値を大きくしないとだめ！
    int karix, kariy;
    public string scenename;
    public int goaldownx, goaldowny, goalupx, goalupy;
    public int startupintx, startupinty, startdownintx, startdowninty;
    Vector3 startup, startdown;
    Vector3 upvectormokuteki, upvectornow, downvectornow, downvectormokuteki = Vector3.zero, directiondown, directionup;//移動にはvectorにする必要がある
                                                                                                                        //-------------------------------------------------
                                                                                                                        //内容としては、玉が動き終わって、スマホの方向、位置が与えられたとき、どの方向に何マス動くかというのを返します
    int Selectrange(int x, int y, int nowx, int nowy)//このx,yはx方向にどれだけ、y方向も同様なので、通常x,y=1,0・0,1・-1,0・0,-1
    {
        int count = 0;
        while (true)
        {
			Debug.Log(nowy + " " + nowx + " " + y + " " + x);
            if (map[nowy + y, nowx + x] == 3)
            {
                karix = nowx; kariy = nowy;
                shougaibutuniatatteruyoflag = true;
                return count;//壁に当たったら、何マス行けたかを返す
            }
            if (map[nowy + y, nowx + x] == 1 && map[nowy + y - x, nowx + x - y] == 1 && map[nowy + y + x, nowx + x + y] == 1)
            {
                karix = nowx; kariy = nowy;
                return count;//壁に当たったら、何マス行けたかを返す
            }
            else if (map[nowy + y, nowx + x] == 1 && map[nowy + y + y, nowx + x + x] == 1 && map[nowy + y + y + y, nowx + x + x + x] == 1) //3連もだめ
            {
                karix = nowx; kariy = nowy;
                return count;//壁に当たったら、何マス行けたかを返す
            }
            nowx += x; nowy += y; count++;
        }
    }

    void selectDirectionandrange(int Direction)//このx,yはgenzaix,genzaiyの意味です。すみません
    {
        //方向を把握していないので、適当に0を上、1を右、2を下、3を左にします
        if (Direction == 2)//上向きの重力
        {
            up = Selectrange(0, 1, nowupx, nowupy);
            nowupx = karix; nowupy = kariy;
            down = Selectrange(0, -1, nowdownx, nowdowny);
            nowdownx = karix; nowdowny = kariy;
        }
        else if (Direction == 1)//右
        {
            up = Selectrange(-1, 0, nowupx, nowupy);
            nowupx = karix; nowupy = kariy;
            down = Selectrange(1, 0, nowdownx, nowdowny);
            nowdownx = karix; nowdowny = kariy;

        }
        else if (Direction == 0)//下
        {
            up = Selectrange(0, -1, nowupx, nowupy);
            nowupx = karix; nowupy = kariy;
            down = Selectrange(0, 1, nowdownx, nowdowny);
            nowdownx = karix; nowdowny = kariy;
        }
        else//左
        {
            up = Selectrange(1, 0, nowupx, nowupy);
            nowupx = karix; nowupy = kariy;
            down = Selectrange(-1, 0, nowdownx, nowdowny);
            nowdownx = karix; nowdowny = kariy;
        }
        //これで代入おーわり。あとはこれに従っていどうして
    }

    bool ballmove()//玉を動かすぞい
    {
        int movexhoukou = 0, moveyhoukou = 0;//下向きがどの方向に行くか、つまり、x=1,y=0で右方向に１進むみたいな
        if (nowrotation == 0) { movexhoukou = 0; moveyhoukou = -1; }//もし、下向きなら!
        if (nowrotation == 1) { movexhoukou = 1; moveyhoukou = 0; }//右!
        if (nowrotation == 2) { movexhoukou = 0; moveyhoukou = 1; }//上!!
        if (nowrotation == 3) { movexhoukou = -1; moveyhoukou = 0; }//左!!
        downvectormokuteki += new Vector3(movexhoukou * haba * down, moveyhoukou * haba * down, 0f);//目的なので、それに方向×距離を足す
                                                                                                    //     upvectormokuteki = ballup.transform.position;//同様
        upvectormokuteki -= new Vector3(movexhoukou * haba * up, moveyhoukou * haba * up, 0f);//同様
		

		/*upvectornow = ballup.transform.position;//今
        downvectornow = balldown.transform.position;//同じ*/

		return true;
    }

    // Use this for initialization
    void Start()
    {
		haba = mapgenerator.GetComponent<setBlock>().ofsScale;
        if (automaticmode)
        {
            map = mapgenerator.GetComponent<automaticgenerator>().map;//これ、こっちの方が速く実行されていたらしぬので、そこを注意
        }
        else
        {
           // map = mapgenerator.GetComponent<codevisualizer>().Lockoff();
        }
        acc = GetComponent<GetAcc>();//GetAccスクリプト
        startup = ballup.transform.position;
        startdown = balldown.transform.position;
        nowrotation = 0;//最初のスマホの角度代入
        upvectormokuteki = ballup.transform.position;
        downvectormokuteki = balldown.transform.position;
        if (automaticmode)//もしおーとまちっくじゃないならもともといれておいて、startは変数nannidoをつかって条件分岐
        {
            goaldownx = mapgenerator.GetComponent<automaticgenerator>().downx;
            goaldowny = mapgenerator.GetComponent<automaticgenerator>().downy;
            goalupx = mapgenerator.GetComponent<automaticgenerator>().nowx;
            goalupy = mapgenerator.GetComponent<automaticgenerator>().nowy;
            startupintx = mapgenerator.GetComponent<automaticgenerator>().startupx;
            startupinty = mapgenerator.GetComponent<automaticgenerator>().startupy;
            startdownintx = mapgenerator.GetComponent<automaticgenerator>().startdownx;
            startdowninty = mapgenerator.GetComponent<automaticgenerator>().startdowny;
        }
        else
        {
            codevisualizer cv = mapgenerator.GetComponent<codevisualizer>();
        }
    }
    bool houkoudetomatteiruka = false;

    bool flaga = true, flagb = true, first = false;
    float tomatteirutaimu = 0;
    // Update is called once per frame
    void Update()
    {
         houkoudetomatteiruka = true; 
        if (pauseflag == false)
        {
            if (gameoverflag == true)
            {//// 
            }
            else
            {
                if (!is_not_moving && pauseflag)
                {
                    return;
                }
                upvectornow = ballup.transform.position;
                downvectornow = balldown.transform.position;

                
                if (SceneManager.GetActiveScene().name == "tutorial")
                {
                    if (is_not_moving == true && acc.getDirection() == tutorialSystem.wantAcc)
                    {
                        Debug.Log(tutorialSystem.wantAcc);
                        if (nowrotation != acc.getDirection())
                        {
                            first = false;
                        }
                        is_not_moving = false;//移動中
                        nowrotation = acc.getDirection();//スマホの角度代入
                        selectDirectionandrange(nowrotation);//上向きに何マス、下向きに何マス移動するかをメモ
                        ballmove();//動かす！
                                   
                    }
                }
                else
                {
                    if (is_not_moving == true && houkoudetomatteiruka && !pauseflag)
                    {//もし移動中じゃないかつスマホの向きが変わっていたら（回転されたら
                        if (nowrotation != acc.getDirection())
                        {
                            first = false;
                        }
                        is_not_moving = false;//移動中
                        nowrotation = acc.getDirection();//スマホの角度代入
                        selectDirectionandrange(nowrotation);//上向きに何マス、下向きに何マス移動するかをメモ
                        ballmove();//動かす！
                    }
                }
		//ここにおそらくアニメーションやらのコードをいれて、トリガー形式にする。下に書いてあるこーどをかえよう
                /*if (Vector3.Distance(upvectormokuteki, upvectornow) >= kyoyouhanni)
                {
                    directionup = (upvectormokuteki - upvectornow).normalized;
                    ballup.transform.Translate(directionup * Time.deltaTime * speed, Space.World);
                }
                else
                {
                    if (flaga)
                    {
                        flaga = false; Debug.Log("flaga");
                        
                    }
                }
                if (Vector3.Distance(downvectormokuteki, downvectornow) >= kyoyouhanni)
                {
                    directiondown = (downvectormokuteki - downvectornow).normalized;
                    balldown.transform.Translate(directiondown * Time.deltaTime * speed, Space.World);
                }
                else
                {
                    if (flagb)
                    {
                        flagb = false; Debug.Log("flagb");
                        
                    }
                }*/
                if (Vector3.Distance(downvectormokuteki, downvectornow) <= kyoyouhanni && Vector3.Distance(upvectormokuteki, upvectornow) <= kyoyouhanni)//スピードを上げたら、この中の値を大きくしないとだめ！
                {
                    if (!first)
                    {
                        flaga = true; flagb = true; first = true;
                    }
                    if (shougaibutuniatatteruyoflag == true)
                    {
                        //    Debug.Log(downvectormokuteki); Debug.Log(downvectornow);
                        restartflag = true;
                        shougaibutuniatatteruyoflag = false;
                    }
                    
                    is_not_moving = true;
                    if (nowdownx == goaldownx && nowdowny == goaldowny && nowupx == goalupx && nowupy == goalupy)
                    {
                        clearflag = true;

                        if (automaticmode)
                        {
                            /*GameObject mapgen = GameObject.Find("mapgenerator");
                            timecounter = GameObject.Find("timecounter");
                            timecounter.GetComponent<timelimitandmemory>().goalupdate(nannido);
                            timecounter.GetComponent<timelimitandmemory>().zenkaivoid();*/
                            SceneManager.LoadScene(scenename);
                        }
                        else
                        {
                            //GameObject clear = GameObject.Find("StageClear");
                            //clear.GetComponent<StageClear>().clear(); //クリア
                            PlayerPrefs.SetInt(CreateButton.sendStageNum.ToString(), 1);
                        }
                    }
                    if (nowdownx == goalupx && nowdowny == goalupy && nowupx == goaldownx && nowupy == goaldowny)
                    {
                        clearflag = true;
                        if (automaticmode)
                        {
                            /*GameObject mapgen = GameObject.Find("mapgenerator");
                            timecounter = GameObject.Find("timecounter");
                            timecounter.GetComponent<timelimitandmemory>().goalupdate(nannido);
                            timecounter.GetComponent<timelimitandmemory>().zenkaivoid();*/
                            SceneManager.LoadScene(scenename);
                        }
                        else
                        {
                            //GameObject clear = GameObject.Find("StageClear");
                            //clear.GetComponent<StageClear>().clear();
                            PlayerPrefs.SetInt(CreateButton.sendStageNum.ToString(), 1);
                        }
                    }
                }


            }
            if (restartflag)
            {
                nowrotation = 0;
                acc.ret = 0;
                acc.lastTouch = 4;
                balldown.transform.position = startdown;
                ballup.transform.position = startup;
                downvectormokuteki = startdown;
                downvectornow = startdown;
                upvectormokuteki = startup;
                upvectornow = startup;
                nowdownx = startdownintx;
                nowupx = startupintx;
                nowdowny = startdowninty;
                nowupy = startupinty;
                restartflag = false;
            }
        }

    }
}
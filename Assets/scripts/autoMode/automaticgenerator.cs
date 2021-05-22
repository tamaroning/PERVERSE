using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-3)]
public class automaticgenerator : MonoBehaviour
{
    //------------------変数-------------------------//
    public int[,] map = new int[50, 50];//x座標、y座標
    bool[,] definiteBlock = new bool[50, 50];//通ることが確定したブロック
    public int nowx, nowy;
    public int downx, downy;
    public int atLeastWallNum;
    public int edgeLength;//一辺の長
    public int rotateNum;
    public int startupx, startdownx, startupy, startdowny;
    int cnt = 0;
    public int previous = 0;//0は下向き、1は右向き、２は下向き、３は左向き
                            //-------------------------------------------------//
                            // Use this for initialization
    public bool isCompeteMode;
    public bool isTutorialMode;
    public string mapCode;

    bool check(int y, int x, int range, int movex, int movey)
    {
        for (int i = 0; i < range; i++)//ゴールにたどり着くまで
        {
            if (map[y + movey, x + movex] == 1 && definiteBlock[y + movey, x + movex] == true)
            {
                return false;
            }
            x += movex; y += movey;
        }

        //    map[y + movey, x + movex] = 1; definiteBlock[y + movey, x + movex] = true;

        for (int i = 0; i < range; i++)
        {
            map[y, x] = 0; definiteBlock[y, x] = true;
            x -= movex; y -= movey;
        }
        return true;
    }
    struct PairWithDirection
    {
        public int x, y, beforeDirection, count;
        public string beforePos;
    };

    PairWithDirection moveToWall(int dir, PairWithDirection now)
    {
        int[] xy = { 0, 1, 0, -1, 0 };
        while (true)
        {
            if (map[now.y + xy[dir + 1], now.x + xy[dir]] != 1)
            {
                now.x += xy[dir];
                now.y += xy[dir + 1];
            }
            else
            {
                return now;
            }
        }

    }
    int conv(PairWithDirection toRed, PairWithDirection toBlue)
    {
        string visCheck = '1' + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0') + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0');
        return int.Parse(visCheck);
    }

    public Dictionary<int, int> vis = new Dictionary<int, int>();
    int CountToGoal(PairWithDirection startBlue, PairWithDirection startRed)
    {
        var blueQueue = new Queue<PairWithDirection>();
        var redQueue = new Queue<PairWithDirection>();

        blueQueue.Enqueue(startBlue);
        redQueue.Enqueue(startRed);
        int finalCnt = -1;
        while (blueQueue.Count > 0)
        {
            PairWithDirection blue = blueQueue.Dequeue();
            PairWithDirection red = redQueue.Dequeue();

            if (map[blue.y, blue.x] == 2 && map[red.y, red.x] == 2)
            {
                finalCnt = blue.count;
                break;
            }
            //Debug.Log(red.x.ToString() + " " + red.y.ToString() + " " + blue.x.ToString() + " " + blue.y.ToString()+" "+red.beforeDirection.ToString());
            for (int toDirection = 0; toDirection < 4; toDirection++)
            {
                if (red.beforeDirection == toDirection)
                {
                    continue;
                }
                var toRed = moveToWall(toDirection, red);
                var toBlue = moveToWall((toDirection + 2) % 4, blue);

                string visCheck = '1' + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0') + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0');
                if (vis.ContainsKey(int.Parse(visCheck))) { continue; }

                vis.Add(int.Parse(visCheck), conv(red, blue));
                visCheck = '1' + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0') + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0');
                vis.Add(int.Parse(visCheck), conv(blue, red));

                toRed.beforeDirection = toBlue.beforeDirection = (toDirection + 2) % 4;
                toRed.count = toBlue.count = toRed.count + 1;

                blueQueue.Enqueue(toBlue);
                redQueue.Enqueue(toRed);
            }
        }
        return finalCnt;
    }

    void Awake()
    {
        Application.targetFrameRate = 60; //60FPSに設定

        if (isCompeteMode || isTutorialMode)
        {
            map = GetComponent<codevisualizer>().Lockoff(mapCode);
			Debug.Log(map[0, 0]);
            return;
        }

        int difficulty = GameObject.Find("pointsText").GetComponent<data>().difficulty;
        if (difficulty == 0)
        {
            nowx = 7;
            nowy = 3;
            downx = 5;
            downy = 5;
            atLeastWallNum = 10;
            edgeLength = 10;
            rotateNum = 10;
        }
        else if (difficulty == 1)
        {
            nowx = 9;
            nowy = 7;
            downx = 7;
            downy = 9;
            atLeastWallNum = 40;
            edgeLength = 16;
            rotateNum = 40;
        }
        else if (difficulty == 2)
        {
            nowx = 13;
            nowy = 11;
            downx = 11;
            downy = 13;
            atLeastWallNum = 80;
            edgeLength = 24;
            rotateNum = 114514;
        }
        else if (difficulty == 3)
        {
            nowx = 11;
            nowy = 9;
            downx = 9;
            downy = 11;
            atLeastWallNum = 60;
            edgeLength = 20;
            rotateNum = 114514;
        }

        map[downx, downy] = 0;
        map[nowx, nowy] = 0;
        startdownx = downx;
        startdowny = downy;
        startupx = nowx;
        startupy = nowy;

        for (int i = 0; i <= edgeLength; i++)
        {
            for (int j = 0; j <= edgeLength; j++)
            {
                definiteBlock[i, j] = false; map[i, j] = 0;
            }
        }
        for (int i = 0; i < 3; i++)//down
        {
            map[nowy - 1, nowx - 1 + i] = 1;
            definiteBlock[nowy - 1, nowx - 1 + i] = true;
        }
        for (int i = 0; i < 3; i++)//up
        {
            map[downy + 1, downx - 1 + i] = 1;
            definiteBlock[downy + 1, downx - 1 + i] = true;
        }
        for (int i = 0; i <= edgeLength; i++)
        {
            map[0, i] = 1; map[i, 0] = 1; map[edgeLength, i] = 1; map[i, edgeLength] = 1;
            definiteBlock[0, i] = true; definiteBlock[i, 0] = true; definiteBlock[edgeLength, i] = true; definiteBlock[i, edgeLength] = true;
        }

        for (int nannkaime = 0; nannkaime < rotateNum; nannkaime++)
        {
            int muki, range = 0, hantairange = 0, downrange = 0, downhantairange = 0;
            //重力通りに動く玉から
            while (true)
            {
                cnt++; if (cnt > 1000) { break; }
                //0は下向き、1は右向き、２は上向き、３は左向き
                muki = Random.Range(0, 4);
                if (muki == previous) { continue; }
                if (muki == (previous + 2) % 4) { continue; }

                if (muki == 0)//した 
                {
                    range = Random.Range(0, edgeLength - 1 - nowy);//壁を作る一歩手前で止まる
                    if (range % 2 == 1) { continue; }//もし距離が奇数なら
                    bool flag = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map[nowy + range + 1, nowx - 1 + i] == 0 && definiteBlock[nowy + range + 1, nowx - 1 + i] == true) { flag = true; break; }//三マス確認
                    }
                    if (flag) { continue; }
                    if (!check(nowy, nowx, range, 0, 1)) { continue; }
                    for (int i = 0; i < 3; i++)
                    {
                        map[nowy + range + 1, nowx - 1 + i] = 1;//三マス塗る！
                        definiteBlock[nowy + range + 1, nowx - 1 + i] = true;//
                    }
                    //----------------------------------------------//
                    nowy += range;//
                    int count2 = 0;
                    while (true)//
                    {
                        count2++; if (count2 > 9999) { break; }
                        downrange = Random.Range(0, downy);
                        if (downrange % 2 == 1) { continue; }
                        flag = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (map[downy - downrange - 1, downx - 1 + i] == 0 && definiteBlock[downy - downrange - 1, downx - 1 + i] == true) { flag = true; break; }//三マス確認
                        }
                        if (flag) { continue; }
                        if (!check(downy, downx, downrange, 0, -1)) { continue; }
                        for (int i = 0; i < 3; i++)
                        {
                            map[downy - downrange - 1, downx - 1 + i] = 1;//直す
                            definiteBlock[downy - downrange - 1, downx - 1 + i] = true;//直す
                        }
                        break;
                    }
                    //----------------------------------------------//
                    downy -= downrange;
                }
                if (muki == 1)
                {
                    range = Random.Range(0, edgeLength - 1 - nowx);//直す
                    if (range % 2 == 1) { continue; }
                    bool flag = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map[nowy - 1 + i, nowx + range + 1] == 0 && definiteBlock[nowy - 1 + i, nowx + range + 1] == true) { flag = true; break; }//三マス確認
                    }
                    if (flag) { continue; }
                    if (!check(nowy, nowx, range, 1, 0)) { continue; }//直す

                    for (int i = 0; i < 3; i++)
                    {
                        map[nowy - 1 + i, nowx + range + 1] = 1;//直す
                        definiteBlock[nowy - 1 + i, nowx + range + 1] = true;//直す
                    }
                    //----------------------------------------------//
                    nowx += range;//直す
                    int count2 = 0;
                    while (true)
                    {
                        count2++; if (count2 > 9999) { break; }
                        downrange = Random.Range(0, downx);//直す
                        if (downrange % 2 == 1) { continue; }
                        flag = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (map[downy - 1 + i, downx - downrange - 1] == 0 && definiteBlock[downy - 1 + i, downx - downrange - 1] == true) { flag = true; break; }//三マス確認
                        }
                        if (flag) { continue; }
                        if (!check(downy, downx, downrange, -1, 0)) { continue; }//直す

                        for (int i = 0; i < 3; i++)
                        {
                            map[downy - 1 + i, downx - downrange - 1] = 1;//直す
                            definiteBlock[downy - 1 + i, downx - downrange - 1] = true;//直す
                        }
                        break;
                    }
                    //----------------------------------------------//
                    downx -= downrange;//直す
                }
                if (muki == 2)//上
                {
                    range = Random.Range(0, nowy);
                    if (range % 2 == 1) { continue; }
                    bool flag = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map[nowy - range - 1, nowx - 1 + i] == 0 && definiteBlock[nowy - range - 1, nowx - 1 + i] == true) { flag = true; break; }//三マス確認
                    }
                    if (flag) { continue; }
                    if (!check(nowy, nowx, range, 0, -1)) { continue; }
                    for (int i = 0; i < 3; i++)
                    {
                        map[nowy - range - 1, nowx - 1 + i] = 1;//直す
                        definiteBlock[nowy - range - 1, nowx - 1 + i] = true;//直す
                    }
                    //----------------------------------------------//
                    nowy -= range;
                    int count2 = 0;
                    while (true)
                    {
                        count2++; if (count2 > 9999) { break; }
                        downrange = Random.Range(0, edgeLength - 1 - downy);//壁を作る一歩手前で止まる
                        if (downrange % 2 == 1) { continue; }//もし距離が奇数なら
                        flag = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (map[downy + downrange + 1, downx - 1 + i] == 0 && definiteBlock[downy + downrange + 1, downx - 1 + i] == true) { flag = true; break; }//三マス確認
                        }
                        if (flag) { continue; }
                        if (!check(downy, downx, downrange, 0, 1)) { continue; }
                        for (int i = 0; i < 3; i++)
                        {
                            map[downy + downrange + 1, downx - 1 + i] = 1;//三マス塗る！
                            definiteBlock[downy + downrange + 1, downx - 1 + i] = true;//
                        }
                        break;
                    }
                    //----------------------------------------------//
                    downy += downrange;//
                }
                if (muki == 3)
                {
                    range = Random.Range(0, nowx);//直す
                    if (range % 2 == 1) { continue; }
                    bool flag = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (map[nowy - 1 + i, nowx - range - 1] == 0 && definiteBlock[nowy - 1 + i, nowx - range - 1] == true) { flag = true; break; }//三マス確認
                    }
                    if (flag) { continue; }
                    if (!check(nowy, nowx, range, -1, 0)) { continue; }//直す

                    for (int i = 0; i < 3; i++)
                    {
                        map[nowy - 1 + i, nowx - range - 1] = 1;//直す
                        definiteBlock[nowy - 1 + i, nowx - range - 1] = true;//直す
                    }
                    //----------------------------------------------//
                    nowx -= range;//直す
                    int count2 = 0;
                    while (true)
                    {
                        count2++; if (count2 > 9999) { break; }
                        downrange = Random.Range(0, edgeLength - 1 - downx);//直す
                        if (downrange % 2 == 1) { continue; }
                        flag = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (map[downy - 1 + i, downx + downrange + 1] == 0 && definiteBlock[downy - 1 + i, downx + downrange + 1] == true) { flag = true; break; }//三マス確認
                        }
                        if (flag) { continue; }
                        if (!check(downy, downx, downrange, 1, 0)) { continue; }//直す

                        for (int i = 0; i < 3; i++)
                        {
                            map[downy - 1 + i, downx + downrange + 1] = 1;//直す
                            definiteBlock[downy - 1 + i, downx + downrange + 1] = true;//直す
                        }
                        break;
                    }
                    //----------------------------------------------//
                    downx += downrange;//直す
                }
                previous = muki;
                break;
            }
        }
        ////////////////////////////////
        string scenename = SceneManager.GetActiveScene().name;
        if (nowx == startupx && nowy == startupy) { SceneManager.LoadScene(scenename); }
        if (nowx == startdownx && nowy == startdowny) { SceneManager.LoadScene(scenename); }
        if (downx == startupx && downy == startupy) { SceneManager.LoadScene(scenename); }
        if (downx == startdownx && downy == startdowny) { SceneManager.LoadScene(scenename); }
        definiteBlock[startupy, startupx] = true;
        definiteBlock[startdowny, startdownx] = true;
        int pikuto = 0;
        for (int i = 1; i < edgeLength; i++)
        {
            for (int j = 1; j < edgeLength; j++)
            {
                if (map[i, j] == 1) { pikuto++; }
            }
        }
        if (pikuto < atLeastWallNum) { SceneManager.LoadScene(scenename); }

        map[nowy, nowx] = 2;
        map[downy, downx] = 2;
        PairWithDirection a = new PairWithDirection(), b = new PairWithDirection();
        a.x = startupx; a.y = startupy; b.x = startdownx; b.y = startdowny; b.beforeDirection = -1;
        int countGoal = CountToGoal(a, b);
        Debug.Log(countGoal);
        if (difficulty == 0 && countGoal <= 1)
        {
            SceneManager.LoadScene(scenename);
        }
        else if (difficulty == 1 && countGoal <= 3)
        {
            SceneManager.LoadScene(scenename);
        }
        else if (difficulty == 3 && countGoal <= 4)
        {
            SceneManager.LoadScene(scenename);
        }
        else if (difficulty == 2 && countGoal <= 4)
        {
            SceneManager.LoadScene(scenename);
        }
        /*for (int i = 0; i < edgeLength; i++) {
            int shougaibutux = Random.Range(0, edgeLength), shougaibutuy = Random.Range(0, edgeLength);
            if (shougaibutux % 2 == 0 || shougaibutuy % 2 == 0) { continue; }
            if (definiteBlock[shougaibutuy, shougaibutux] == false) {

                definiteBlock[shougaibutuy, shougaibutux] = true;
                map[shougaibutuy, shougaibutux] = 3;
            }
        }*/

        var movetheball = GameObject.Find("playerObjectUp").GetComponent<movePlayer>();
        movetheball.goaldownx = downx;
        movetheball.goaldowny = downy;
        movetheball.goalupx = nowx;
        movetheball.goalupy = nowy;
		if (SceneManager.GetActiveScene().name == "bot") {
			GameObject.Find("pointsText").GetComponent<data>().botGameClear();
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
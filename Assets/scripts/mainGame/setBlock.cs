using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[DefaultExecutionOrder(-2)]
public class setBlock : MonoBehaviour
{

    static Color ToColor(string self)
    {
        var color = default(Color);
        if (!ColorUtility.TryParseHtmlString(self, out color))
        {
            Debug.LogWarning("Unknown color code... " + self);
        }
        return color;
    }

    int not(int a)
    {
        if (a == 0)
        {
            return 1;
        }
        return 0;
    }
    /*
    int flr(float a){
        return Mathf.Floor(a*100.0F)*100.0F;
    }*/


    //マップの範囲外じゃないかチェック
    bool outCheck(int y, int x)
    {
        if (x < 0 || y < 0) { return true; }
        if (width < x || height < y) { return true; }
        return false;
    }

    GameObject createBlockObj(float px, float py, int z, Sprite sp, int colorIndex, int w, int h)
    {

        //sprite表示処理をかく
        GameObject obj = blockPrefab;

        obj = Instantiate(blockPrefab, obj.transform.position, obj.transform.rotation);
        //子オブジェクトにする
        obj.transform.SetParent(transform);
        //obj.GetComponent<RectTransform>().position = new Vector3((px - ofsX ) * ofsScale, (py - ofsY) * ofsScale, 0);
        obj.transform.position = new Vector3((px + 1F) * 100F * ofsScale, (py + ofsY) * 100F * ofsScale, 0);

        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(w * ofsScale * 100F, h * ofsScale * 100F);
        obj.GetComponent<Image>().color = blockColorArr[blockColorIndex, colorIndex];
        assignSprite(obj, sp);

        rotZ(obj, z);

        obj.transform.SetAsFirstSibling();
        return obj;
    }

    void assignSprite(GameObject gobj, Sprite sp)
    {

        gobj.GetComponent<Image>().sprite = sp;
        //sprite = sp;

        return;
    }

    void rotZ(GameObject obj, float rot)
    {

        obj.transform.rotation = Quaternion.Euler(0, 0, rot);

        return;
    }




    //まっすぐなブロックを配置
    void placeBlockStraight(int px, int py, int lx, int ly, int colorIndex)
    {
        Sprite sp = corner[0];//Nullはダメなのでいれておく
        colorSwitch = UnityEngine.Random.Range(0, 3);
        if (colorSwitch == 2) colorSwitch = 0;
        int z = 0;//z軸回転の角度
        int w = 1, h = 1;

        if ((lx == 1 && ly == 0) || (lx == 0 && ly == 1))
        {
            //縦横1マス
            sp = square[colorSwitch];
        }
        else if (lx == 2)
        {
            //横長2マス
            sp = straight2[colorSwitch];
            w = 2;
        }
        else if (lx == 3)
        {
            //横長3マス
            sp = straight3[colorSwitch];
            w = 3;
        }
        else if (ly == 2)
        {
            //縦長2マス
            sp = straight2[colorSwitch];
            w = 2;
            //回転
            z = 90;
            px++;
        }
        else if (ly == 3)
        {
            //縦長3マス
            sp = straight3[colorSwitch];
            w = 3;
            //回転
            z = 90;
            px++;
        }
        GameObject block = createBlockObj(px, py, z, sp, colorIndex, w, h);
        return;
    }


    //角のブロックを配置
    //corner 0:左上 1:右上 2:左下 3:右下
    //shape 0:2*2 1:横長　2:縦長
    void placeBlockCorner(int cornerNum, int shape, int colorIndex)
    {
        //位置とsprite設定
        int px = 0, py = 0;
        int z = 0;//z軸回転の角度
        int w = 2, h = 2;
        Sprite sp = null;
        colorSwitch = UnityEngine.Random.Range(0, 2);
        if (cornerNum == 0)
        {
            px = 0;
            py = 0;
            if (shape == 0)
            {
                //sp=2*2
                sp = corner[colorSwitch];
            }
            else if (shape == 1)
            {
                //sp=横長L
                sp = cornerVlong[colorSwitch];
                w = 3;
            }
            else if (shape == 2)
            {
                //sp=縦長L
                sp = cornerHlong[colorSwitch];
                h = 3;
            }

        }
        else if (cornerNum == 1)
        {
            px = width - 1;
            py = 0;
            if (shape == 0)
            {
                //sp=2*2
                sp = corner[colorSwitch];
            }
            else if (shape == 1)
            {
                //sp=横長L
                sp = cornerHlong[colorSwitch];
                h = 3;
            }
            else if (shape == 2)
            {
                //sp=縦長L
                sp = cornerVlong[colorSwitch];
                w = 3;
            }
            //90度回転処理
            z = 90;
            px++;

        }
        else if (cornerNum == 2)
        {
            px = 0;
            py = height - 1;
            if (shape == 0)
            {
                //sp=2*2
                sp = corner[colorSwitch];
            }
            else if (shape == 1)
            {
                //sp=横長L
                sp = cornerHlong[colorSwitch];
                h = 3;
            }
            else if (shape == 2)
            {
                //sp=縦長L
                sp = cornerVlong[colorSwitch];
                w = 3;
            }
            //180度回転
            z = 270;
            py++;
        }
        else if (cornerNum == 3)
        {
            px = width - 1;
            py = height - 1;
            if (shape == 0)
            {
                //sp=2*2
                sp = corner[colorSwitch];
            }
            else if (shape == 1)
            {
                //sp=横長L
                sp = cornerVlong[colorSwitch];
                w = 3;
            }
            else if (shape == 2)
            {
                //sp=縦長L
                sp = cornerHlong[colorSwitch];
                h = 3;
            }
            //270度回転
            z = 180;
            py++;
            px++;

        }

        GameObject block = createBlockObj(px, py, z, sp, r2, w, h);
        return;
    }

    public GameObject mainCamera;
    public GameObject blockPrefab;

    //Vlong:縦長 Hlong:横長
    //塗られてる
    public Sprite squareFill;
    public Sprite straight2Fill;
    public Sprite straight3Fill;

    public Sprite cornerFill;
    public Sprite cornerVlongFill;
    public Sprite cornerHlongFill;
    //枠線
    public Sprite squareFrame;
    public Sprite straight2Frame;
    public Sprite straight3Frame;

    public Sprite cornerFrame;
    public Sprite cornerVlongFrame;
    public Sprite cornerHlongFrame;
    //実際に使う方
    Sprite[] square = new Sprite[2];
    Sprite[] straight2 = new Sprite[2];
    Sprite[] straight3 = new Sprite[2];

    Sprite[] corner = new Sprite[2];
    Sprite[] cornerVlong = new Sprite[2];
    Sprite[] cornerHlong = new Sprite[2];

    Color[,] blockColorArr;
    //set 3color
    Color[] blockColor = new Color[3] { Color.blue, Color.red, Color.green };

    //ブロック設置の際、隣のブロックとかぶらないようにする
    int[] nextColorCount = new int[] { 0, 0, 0, 0, 0, 0, 0 };

    int[] dx = new int[] { 1, 0, -1, 0 };
    int[] dy = new int[] { 0, 1, 0, -1 };

    int patCount = 0;
    int[,] pat = new int[,] {
        {0,1,2},
        {0,2,1},
        {1,0,2},
        {1,2,0},
        {2,0,1},
        {2,1,0}
    };
    public float displayWidth, displayHeight;
    int width = 17, height = 17;
    int blockColorIndex = 0;
    int count = 2;
    int emp, maxemp, maxempi;
    int nxtX, nxtY;
    int nowX, nowY;
    int plX, plY;//ブロックを置く位置(一番左上のブロックが基準)
    int lenX, lenY;//ブロックの長さ(これでブロックの種類を決定する)
    int r, r2, colorSwitch;//乱数用

    public float ofsX, ofsY;
    public float ofsScale;

    public automaticgenerator automaticScript;
    //map[,]に表示させたいマップデータを入れる
    /*int[,] map = new int[,]{
    //   1       5         10        15        20
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,0,1,0,0,1,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,1,0,0,0,0,1,1,1,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0},
        {1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,1,1,1,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,1,1,0,0,0,0,1,0,0,0,1,0,1,1,1,0,0,0,0,0},
        {1,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,1,1,0,0,0,0,0,0,0,0,1,0,0,2,1,0,0,0,0,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
    };
    */
    int[,] map = new int[50, 50];
    // Use this for initialization
    void Start()
    {

        int difficulty = GameObject.Find("pointsText").GetComponent<data>().difficulty;

        if (difficulty == 0)
        {
            width = height = 11;
        }
        else if (difficulty == 1)
        {
            width = height = 17;
        }
        else if (difficulty == 2)
        {
            width = height = 25;
        }
        else
        {
            width = height = 21;
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j] = automaticScript.map[i, j];
            }
        }

        r = UnityEngine.Random.Range(0, 2);
        r = 1;
        square[r] = squareFill;
        straight2[r] = straight2Fill;
        straight3[r] = straight3Fill;
        corner[r] = cornerFill;
        cornerVlong[r] = cornerVlongFill;
        cornerHlong[r] = cornerHlongFill;

        r = not(r);

        square[r] = squareFrame;
        straight2[r] = straight2Frame;
        straight3[r] = straight3Frame;
        corner[r] = cornerFrame;
        cornerVlong[r] = cornerVlongFrame;
        cornerHlong[r] = cornerHlongFrame;

        blockColorArr = new Color[,]{
            {ToColor("#0ba6dc"),ToColor("#4fc3f7"),ToColor("#7bddff")},//青
            {ToColor("#b37dc7"),ToColor("#ddc3fb"),ToColor("#c6a6e1")},//紫
            {ToColor("#faccf0"),ToColor("#eaa5d3"),ToColor("#f9c4ee")},//ピンク
            {ToColor("#d8ca64"),ToColor("#e6e79d"),ToColor("#e6e3a9")},//黄色
            {ToColor("#9fd66e"),ToColor("#c5e1a5"),ToColor("#97d178")},//緑
            {ToColor("#f09b7a"),ToColor("#a2a3a6"),ToColor("#d5dcdd")},//灰色
            {ToColor("#0ba6dc"),ToColor("#4fc3f7"),ToColor("#7bddff")}
        };

        blockColorIndex = GameObject.Find("pointsText").GetComponent<data>().colorNum;
        for (int i = 0; i < 3; i++)
        {
            blockColor[i] = blockColorArr[blockColorIndex, i];
        }

        //表示位置合わせ
        // displayWidth = (int)Screen.width;//画面サイズ取得
        displayHeight = (int)Screen.height;
        displayWidth = 1440;


        //ofsX = (width + 2) / 2.0F - 1.0F;//1.0fで画面端から1ブロック分間隔をとる
        ofsY = height / 2 - 1f;//1.5ぶろっく分、マップ全体を下に表示

        ofsScale = (displayWidth / 100F) / (width + 2f);//1マスのpixelを計算
        //Debug.Log(ofsScale);

        //他の大きさ系対応
        float sizeSetDifficulty = (float)17 / (float)height;
        GameObject.Find("goalOutMask").GetComponent<RectTransform>().sizeDelta = new Vector3(ofsScale * (width) * 100f + 5f, ofsScale * (width) * 100f + 5f);
        //Debug.Log(ofsY);
        GameObject.Find("goalOutMask").GetComponent<RectTransform>().localPosition = new Vector3(0, 1);

        GameObject.Find("goalOutMask").transform.position = new Vector3(GameObject.Find("goalOutMask").transform.position.x, (ofsY + height / 2.0f) * 100f * ofsScale);
        GameObject.Find("playerObjectUp").transform.localScale = new Vector3(1.3f * sizeSetDifficulty, 1.3f * sizeSetDifficulty);
        GameObject.Find("playerObjectDown").transform.localScale = new Vector3(1.3f * sizeSetDifficulty, 1.3f * sizeSetDifficulty);

        //ここまで
        for (int iy = 0; iy < height; iy++)
        {
            for (int ix = 0; ix < width; ix++)
            {
                int swap = map[(height - 1 - iy), ix];
                map[(height - 1 - iy), ix] = map[iy, ix];
                map[iy, ix] = swap;
            }
        }

        int goalCount = 0;
        for (int iy = 0; iy < height; iy++)
        {
            for (int ix = 0; ix < width; ix++)
            {
                if (map[iy, ix] == 2)
                {
                    //ゴールだった場合
                    GameObject goal;
                    if (goalCount == 0)
                    {
                        //１つ目のゴール
                        goal = GameObject.Find("goal1");
                        goalCount++;
                    }
                    else
                    {
                        //2つめのゴール
                        goal = GameObject.Find("goal2");
                    }
                    //Debug.Log("goal " + ix + "," + iy);
                    goal.transform.position = new Vector3((float)(ix + 1 + 0.5) * 100 * ofsScale, (float)(iy + 0.5 + ofsY) * 100 * ofsScale, 0);
                    goal.transform.localScale = new Vector3(sizeSetDifficulty, sizeSetDifficulty, 1F);
                }

            }
        }


        //ofsScale = Mathf.Floor(ofsScale / 100) * 100;

        //camera init
        //mainCamera.transform.position = new Vector3(width / 2, height / 2, mainCamera.transform.position.z);
        //camera.GetComponent<Camera>().size = 10;

        for (int i = 0; i < 4; i++)
        {
            r = UnityEngine.Random.Range(0, 3);//形
            r2 = UnityEngine.Random.Range(0, 3);//色

            //Debug.Log("corner " + i + "shape " + r + "corner " + r2 + "count " + count);
            count = r2 + 2;

            if (i == 0)
            {
                map[0, 0] = count;
                map[0, 1] = count;
                map[1, 0] = count;//左上
            }
            else if (i == 1)
            {
                map[0, width - 1] = count;
                map[0, width - 2] = count;
                map[1, width - 1] = count;//右上
            }
            else if (i == 2)
            {
                map[height - 1, 0] = count;
                map[height - 1, 1] = count;
                map[height - 2, 0] = count;//左下
            }
            else
            {
                map[height - 1, width - 1] = count;
                map[height - 1, width - 2] = count;
                map[height - 2, width - 1] = count;//右下
            }

            if (r == 0)
            {

            }
            else if (r == 1)
            {//横長4ブロック
                if (i == 0)
                {
                    map[0, 2] = count;
                }
                else if (i == 1)
                {
                    map[0, width - 3] = count;
                }
                else if (i == 2)
                {
                    map[height - 1, 2] = count;
                }
                else
                {
                    map[height - 1, width - 3] = count;
                }

            }
            else if (r == 2)
            {//縦長4ブロック
                if (i == 0)
                {
                    map[2, 0] = count;
                }
                else if (i == 1)
                {
                    map[2, width - 1] = count;
                }
                else if (i == 2)
                {
                    map[height - 3, 0] = count;
                }
                else
                {
                    map[height - 3, width - 1] = count;
                }
            }
            placeBlockCorner(i, r, r2);

        }
        for (int iy = 0; iy < height; iy++)
        {
            for (int ix = 0; ix < width; ix++)
            {


                if (map[iy, ix] != 1) { continue; }
                //map[ix][iy]==1の処理
                nowX = ix;
                nowY = iy;

                maxemp = 1;
                maxempi = 0;

                //2傍を調べる
                for (int i = 0; i < 2; i++)
                {
                    nextColorCount[0] = 0;
                    nextColorCount[1] = 0;
                    nextColorCount[2] = 0;
                    emp = 0;
                    //隣とその奥を調べる
                    for (int j = 0; j < 3; j++)
                    {

                        //i方向にi進んだ場所を設定
                        nxtX = nowX + dx[i] * (j);
                        nxtY = nowY + dy[i] * (j);

                        //そこが未探索か調査
                        if (outCheck(nxtY, nxtX))
                        {//そもそも範囲外ならbreak
                            break;
                        }

                        if (map[nxtY, nxtX] != 1)
                        {//探索済みならbreak
                            break;
                        }

                        emp++;//空きマスとみなす

                        for (int k = 0; k < 4; k++)
                        {
                            if (outCheck(nxtY + dy[k], nxtX + dx[k]))
                            {
                                continue;
                            }
                            else if (2 <= map[nxtY + dy[k], nxtX + dx[k]])
                            {
                                //Debug.Log((nxtY + dy[k])+","+ (nxtX + dx[k]));
                                //Debug.Log(map[nxtY + dy[k], nxtX + dx[k]] - 2);
                                nextColorCount[map[nxtY + dy[k], nxtX + dx[k]] - 2] += 1;
                            }
                        }
                    }
                    //探索の最高記録更新
                    if (emp > maxemp)
                    {
                        maxemp = emp;
                        maxempi = i;
                    }
                }
                //いろきめ
                count = -1;
                patCount++;
                if (patCount == 6)
                {
                    patCount = 0;
                }
                for (int i = 0; i < 3; i++)
                {
                    if (nextColorCount[pat[patCount, i]] == 0)
                    {
                        count = pat[patCount, i];
                        break;
                    }
                }
                if (count == -1)
                {
                    count = UnityEngine.Random.Range(0, 3);
                }

                //ブロックの設置
                plX = nowX;
                plY = nowY;
                lenX = 0;
                lenY = 0;
                r = UnityEngine.Random.Range(1, maxemp + 1);

                //始点からi方向をrマスをfill
                for (int j = 0; j < r; j++)
                {
                    lenX += dx[maxempi];
                    lenY += dy[maxempi];
                    map[nowY + dy[maxempi] * (j), nowX + dx[maxempi] * (j)] = count + 2;
                }

                //ブロックを生成
                //Debug.Log(lenX + "," + lenY);
                placeBlockStraight(plX, plY, lenX, lenY, count);

                count++;

            }
        }

        //backGround Grid
        //testPlace();

        string dblog = "";
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (map[height - j - 1, i] == 0)
                {
                    dblog += "  ,";
                }
                else
                {
                    dblog += map[height - j - 1, i].ToString() + ",";
                }
            }
            dblog += "\n";
        }
        Debug.Log(dblog);


    }

    // Update is called once per frame
    void Update()
    {

    }
}


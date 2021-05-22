using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapgenerator : MonoBehaviour
{
    public bool automaticMode;
    public int width;
    //Comment To Pikuto
    //map配列では、３つ1が連なることによって壁が作られます。縦に3つ並んでいた場合は縦の壁が、横の場合は横が、という感じです。
    /*public int[,] map = new int[,] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,1,0,1,1,1,1,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,1,1,1,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,1,1,1,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
    };*/
    public int[,] map = new int[50, 50];
    public GameObject verticalObject_Instantiate, landscapeObject_Instantiate, goalObject_Instantiate, obstacleObject_Instantiate;
    public float leftMostX, leftMosty, interval_each_block, leftMostZ;//leftMostはマップの左端のx座標またはy座標版、interval_each_blockは、一ますごとの間隔
    // Use this for initialization
    void Start()
    {
        float shiftZAxis = 0;//微妙なずれを発生させてテクスチャをよくするため
        leftMostX = -5; leftMosty = -5; interval_each_block = 1;
        if (automaticMode)
            map = GetComponent<automaticgenerator>().map;//これ、こっちの方が速く実行されていたらしぬので、そこを注意
        else
        {
           // map = GetComponent<codevisualizer>().Lockoff();
            width = GetComponent<codevisualizer>().width + 1;
        }
        //縦に連なった壁
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (x % 2 == 1) { continue; }
                int yMiddleValue = y / 2, xMiddleValue = x / 2;
                if (map[x, y] == 1 && map[x + 1, y] == 1 && map[x + 2, y] == 1)
                {
                    shiftZAxis += 0.001f;
                    GameObject instantVertical = (GameObject)Instantiate(verticalObject_Instantiate);//縦方向の壁を複製
                    instantVertical.transform.position = new Vector3(-leftMostX - interval_each_block * (width / 2 - yMiddleValue + 1), leftMosty + interval_each_block * (width / 2 - xMiddleValue + 1) - 0.5f, leftMostZ + shiftZAxis);
                }
            }
        }
        //横に連なった壁
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {

                int yMiddleValue = y / 2, xMiddleValue = x / 2;
                if (map[x, y] == 2)
                {
                    GameObject goalInstantiate = (GameObject)Instantiate(goalObject_Instantiate);
                    goalInstantiate.transform.position = new Vector3(-leftMostX - interval_each_block * (width / 2 + 1 - yMiddleValue) + 0.5f, leftMosty + interval_each_block * (width / 2 - xMiddleValue + 1) - 0.5f, 0f);
                    continue;
                }
                if (map[x, y] == 3)
                {
                    GameObject shougaibutu = (GameObject)Instantiate(obstacleObject_Instantiate);
                    shougaibutu.transform.position = new Vector3(-leftMostX - interval_each_block * (width / 2 + 1 - yMiddleValue) + 0.5f, leftMosty + interval_each_block * (width / 2 - xMiddleValue + 1) - 0.5f, 0f);
                    continue;

                }
                if (y % 2 == 1) { continue; }
                if (map[x, y] == 1 && map[x, y + 1] == 1 && map[x, y + 2] == 1)
                {
                    shiftZAxis += 0.001f;
                    GameObject instantLandscape = (GameObject)Instantiate(landscapeObject_Instantiate);//横方向の壁を複製
                    instantLandscape.transform.position = new Vector3(-leftMostX - interval_each_block * (width / 2 + 1 - yMiddleValue) + 0.5f, leftMosty + interval_each_block * (width / 2 - xMiddleValue + 1), leftMostZ + shiftZAxis);
                }
            }
        }
    }

}

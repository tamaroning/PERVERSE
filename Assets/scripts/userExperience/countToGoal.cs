using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class countToGoal : MonoBehaviour {

	int[,] map;


	struct PairWithDirection {
		public int x, y, beforeDirection, count;
		public string beforePos;
	};

	PairWithDirection moveToWall(int dir, PairWithDirection now) {
		int[] xy = { 0, 1, 0, -1, 0 };
		while (true) {
			if (map[now.y + xy[dir + 1], now.x + xy[dir]] != 1) {
				now.x += xy[dir];
				now.y += xy[dir + 1];
			}
			else {
				return now;
			}
		}

	}

	int nowupx, nowdownx, nowupy, nowdowny;

	int conv(PairWithDirection toRed, PairWithDirection toBlue) {
		string visCheck = '1' + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0') + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0');
		return int.Parse(visCheck);
	}

	public Dictionary<int, int> vis = new Dictionary<int, int>();
	int CountToGoal(PairWithDirection startBlue, PairWithDirection startRed) {
		var blueQueue = new Queue<PairWithDirection>();
		var redQueue = new Queue<PairWithDirection>();

		blueQueue.Enqueue(startBlue);
		redQueue.Enqueue(startRed);
		int finalCnt = -1;
		while (blueQueue.Count > 0) {
			PairWithDirection blue = blueQueue.Dequeue();
			PairWithDirection red = redQueue.Dequeue();

			if (map[blue.y, blue.x] == 2 && map[red.y, red.x] == 2) {
				finalCnt = blue.count;
				break;
			}
			//Debug.Log(red.x.ToString() + " " + red.y.ToString() + " " + blue.x.ToString() + " " + blue.y.ToString()+" "+red.beforeDirection.ToString());
			for (int toDirection = 0; toDirection < 4; toDirection++) {
				if (red.beforeDirection == toDirection) {
					continue;
				}
				var toRed = moveToWall(toDirection, red);
				var toBlue = moveToWall((toDirection + 2) % 4, blue);

				string visCheck = '1' + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0') + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0');
				if (vis.ContainsKey(int.Parse(visCheck))) { continue; }

				vis.Add(int.Parse(visCheck), conv(red,blue));
				visCheck = '1' + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0') + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0');
				vis.Add(int.Parse(visCheck), conv(blue,red));

				toRed.beforeDirection = toBlue.beforeDirection = (toDirection + 2) % 4;
				toRed.count = toBlue.count = toRed.count + 1;

				blueQueue.Enqueue(toBlue);
				redQueue.Enqueue(toRed);
			}
		}
		return finalCnt;
	}

	int DirectionToGoal(PairWithDirection startBlue, PairWithDirection startRed,int count) {

		int foundcount = 0;
		PairWithDirection goalRed = new PairWithDirection(), goalBlue=new PairWithDirection();
		for (int i = 0; i < 50; i++) {
			for (int j = 0; j < 50; j++) {
				if (map[i, j] == 2) {
					if (foundcount == 0) {
						foundcount++;
						goalBlue.x = j;
						goalBlue.y = i;
					}
					else {
						goalRed.x = j;
						goalRed.y = i;
					}
					//Debug.Log(i + " " + j);
				}
			}
		}
		int goal = conv(goalBlue, goalRed);
		for(int i = 0; i < count - 1; i++) {
			goal = vis[goal];
		}
		for (int toDirection = 0; toDirection < 4; toDirection++) {
			var toRed = moveToWall(toDirection, startRed);
			var toBlue = moveToWall((toDirection + 2) % 4, startBlue);

			string visCheck = '1' + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0') + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0');
			if (visCheck == goal.ToString()) {
				return toDirection;
			}
			visCheck = '1' + toBlue.x.ToString().PadLeft(2, '0') + toBlue.y.ToString().PadLeft(2, '0') + toRed.x.ToString().PadLeft(2, '0') + toRed.y.ToString().PadLeft(2, '0');
			if (visCheck == goal.ToString()) {
				return toDirection;
			}
		}
		return -1;

	}

	PairWithDirection a, b;
	public int cnt(int ax, int ay, int bx, int by) {
		a.x = ax; a.y = ay; b.x = bx; b.y = by; b.beforeDirection = -1;
		vis.Clear();
		return CountToGoal(a, b);
	}

	public int whichDirectionToGo(int ax, int ay, int bx, int by,int count) {
		string dblog = "";
		for (int j = 0; j < 20; j++) {
			for (int i = 0; i < 20; i++) {
				if (map[20 - j - 1, i] == 0) {
					dblog += "  ,";
				}
				else {
					dblog += map[20 - j - 1, i].ToString() + ",";
				}
			}
			dblog += "\n";
		}
		//Debug.Log(dblog);

		a.x = ax; a.y = ay; b.x = bx; b.y = by; b.beforeDirection = -1;
		return DirectionToGoal(a, b,count);

	}

	private void Awake() {
		map = GetComponent<automaticgenerator>().map;
		int difficulty = GameObject.Find("pointsText").GetComponent<data>().difficulty;
		if (difficulty == 0) {
			nowupx = 7;
			nowdownx = 5;
			nowupy = 3;
			nowdowny = 5;
		}
		if (difficulty == 1) {
			nowupx = 9;
			nowdownx = 7;
			nowupy = 7;
			nowdowny = 9;
		}
		if (difficulty == 2) {
			nowupx = 13;
			nowdownx = 11;
			nowupy = 11;
			nowdowny = 13;
		}
		if (difficulty == 3) {
			nowupx = 11;
			nowupy = 9;
			nowdownx = 9;
			nowdowny = 11;
		}
		a.x =nowdownx; a.y = nowdowny; b.x = nowupx; b.y = nowupy; b.beforeDirection = -1;
		GameObject.Find("flagNum").GetComponent<Text>().text = CountToGoal(a, b).ToString();
	}
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
public class codemaker : MonoBehaviour {
	public string code = "";
	//easyなら1、normalなら2、hardなら3
	public GameObject automatic;
	automaticgenerator script;
	private static List<char> rokujuuyonnlist = new List<char>(){//256進数！！
   'Σ','Τ','Υ','Φ','Χ','Ψ','Ω','α','β','γ','δ','ε','ζ','η','θ','ι','κ','λ','μ','ν','ξ','ο','π','ρ','σ','τ','υ','φ','χ','ψ','ω', '0','≠','≦','≧','＜','＞','≪','≫','∞','∽','∝','∴','∵','∈','∋','⊆','⊇','⊂','⊃','∪','∩','∧','∨','￢','⇒','⇔','∀','∃','∠','⊥','⌒','∂','∇','≡','√','∫','∬','─','│','┌','┐','┘','└','├','┬','┤','┴','┼','━','┃','┏','┓','┛','┗','┣','┳','┫','┻','╋','┠','┯','┨','┷','┿','┝','┰','┥','┸','╂','＃','＆','＊','＠','§','※','〓','♯','♭','♪','†','‡','¶','仝','々','〆','ー','～','￣','＿','―','‐','∥','｜','／','＼' ,'1', '2', '3', '4', '5', '6', '7', '8', '9','a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j','k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N','O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X','Y', 'Z', '+',
		'Α','Β','Γ','Δ','Ε','Ζ','Η','Θ','Ι','Κ','Λ','Μ','Ν','Ξ','Ο','Π','Ρ','А','Б','В','Г','Д','Е','Ё','Ж','З','И','Й','К','Л','М','Н','О','П','Р','С','Т','У','Ф','Х','Ц','Ч','Ш','Щ','Ъ','Ы','Ь','Э','Ю','Я','а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ы','ь','э','ю','я',
		'＋','－','±','×','÷','＝','≒'
  };
	public static string nisinnsuukara64sinnsuu(string num) {
		int digit = (1 + ((num.Length - 1) / 8)) * 8;
		num = num.PadRight(digit, '0');
		string rokujuuyon = "";
		for (int i = 0; i < num.Length; i += 8) {
			int no = Convert.ToInt32(num.Substring(i, 8), 2);
			rokujuuyon += rokujuuyonnlist[no];
		}
		return rokujuuyon;
	}

	public string codegenerate(int width)//widthは偶数
	{
		var goal = new Stack<int>();
		var shougaibutu = new List<int>();
		script = automatic.GetComponent<automaticgenerator>();
		int[,] map = script.map;
		code += "" + (width + 1);
		string ichigyou = "";
		if (width == 0) {
			width = 10;
		}
		else if (width == 1) {
			width = 16;
		}
		else if (width == 2) {
			width = 24;
		}
		else {
			width = 20;
		}
		for (int i = 1; i < width; i++) {
			for (int j = 1; j < width; j++) {
				if (map[i, j] == 2) { goal.Push(i); goal.Push(j); continue; }
				//if (map[i, j] == 3) { shougaibutu.Add(i); shougaibutu.Add(j);continue; }
				if (i % 2 == 0 && j % 2 == 1) {
					ichigyou += map[i, j].ToString();
				}
				else if (i % 2 == 1 && j % 2 == 0) {
					ichigyou += map[i, j].ToString();
				}
			}
		}
		code += nisinnsuukara64sinnsuu(ichigyou);
		code += "#";
		for (int i = 0; i < 2; i++) {
			int x = goal.Pop();
			int y = goal.Pop();
			y /= 2; x /= 2;
			code += rokujuuyonnlist[y * 13 + x];
		}
		int size = shougaibutu.Count;
		for (int i = 0; i < size; i += 2) {
			code += rokujuuyonnlist[shougaibutu[i] / 2 * 13 + shougaibutu[i + 1] / 2];
		}
		return code;
	}

}

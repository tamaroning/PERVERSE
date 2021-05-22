using System;
using System.IO;
using UnityEngine;

/// <summary>
/// エラーログをファイルに出力する
///・空の GameObject にアタッチして使う（DontDestroyOnLoad によりシーン変更でも残る[※本来はシングルトンモデルにした方が良い]）。
///・ログは内部ストレージの "Android/data/[PackageName]/files/～" に出力
///（機種によっては外部ストレージになることもあるらしい）。
///・Debug.LogError() も保存される（LogType が Error または Exception で判断する）。
///・「Build Settings」の「Development Build」「Script Debugging」オンにすれば行番号も出力できる。
/// </summary>
public class ErrorReporter : MonoBehaviour {

	public string reportFileName = "error_report.txt";  //出力するファイル名（任意）
	public bool addDateTime = false;                    //ファイル名に日時を付加する

	public bool typeException = true;       //Exception のスタックトレースを保存する
	public bool typeError = true;           //Debug.LogError() を保存する

	void OnEnable() {
		//Application.RegisterLogCallback(HandleLog);  //obsolute
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable() {
		//Application.RegisterLogCallback(null);  //obsolute
		Application.logMessageReceived -= HandleLog;
	}

	//ログハンドリンング
	void HandleLog(string condition, string stackTrace, LogType type) {
		if ((typeException && type == LogType.Exception) || (typeError && type == LogType.Error)) {
			DateTime dt = DateTime.Now;
			string text = dt.ToString("[yyyy-MM-dd HH:mm:ss]")
				+ "\ncondition : " + condition + "\nstackTrace : " + stackTrace.Trim() + "\ntype : "
				+ type.ToString() + "\n";

			string outfile = reportFileName;
			if (addDateTime) {
				string file = Path.GetFileNameWithoutExtension(reportFileName);
				string ext = Path.GetExtension(reportFileName); //"."を含む拡張子
				outfile = file + "_" + dt.ToString("yyyyMMddHHmmss") + ext;
			}

			SaveText(text, Path.Combine(Application.persistentDataPath, outfile));
		}
	}

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(this);
	}

	// テキストファイルファイル保存
	public static bool SaveText(string text, string path) {
		try {
			using (StreamWriter writer = new StreamWriter(path, true)) {
				writer.Write(text);
				writer.Flush();
				writer.Close();
			}
		}
		catch (Exception e) {
			Debug.Log(e.Message);
			return false;
		}
		return true;
	}
}
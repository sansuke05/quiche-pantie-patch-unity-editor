using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Communication {
	private const string URL = "http://pantie-patch.herokuapp.com/api/dream/0001.png";

	private WWW www;
	
	FileCreator creator;

	public Communication() {
		www = new WWW(URL);
	}

	public string GetTexture() {
		Texture2D tex;
		var fileName = "0001.png";

		www.MoveNext();

		// リクエストが完了した時の処理
		if (www.isDone) {
			tex = www.texture;
			// テクスチャデータの保存
			creator = new FileCreator();
			creator.Create(fileName, tex);
			
			return "finished";
		}

		return "";
	}

	public float GetProgress() {
		return www.progress;
	}

	public void Clear() {
		www.Dispose();
	}
}

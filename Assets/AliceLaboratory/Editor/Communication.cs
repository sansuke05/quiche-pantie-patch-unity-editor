using System;
using UnityEngine;
using System.Collections;
using AliceLaboratory.Editor;
using UnityEngine.Networking;

public class Communication {
	private const string DREAMS_BASE_URL = "http://pantie-patch.herokuapp.com/api/dream/";

	private WWW www;
	
	FileCreator creator;

	public Communication() {
		www = new WWW(DREAMS_BASE_URL);
	}

	// ステート管理に変更予定
	public Dream GetDreams() {
		Dream dream = null;
		
		www.MoveNext();
		
		// リクエストが完了した時の処理
		if (www.isDone) {
			dream = JsonUtility.FromJson<Dream>(www.text);
		}

		/*
		if (dream != null) {
			// テクスチャデータを取得
			foreach (var image in dream.images) {
				www = new WWW(DREAMS_BASE_URL + image);
				while (true) {
					if (GetTexture(image) == "finished") {
						break;
					}
				}
			}
		}*/
		return dream;
	}

	public string GetTexture(string fileName) {
		Texture2D tex;

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

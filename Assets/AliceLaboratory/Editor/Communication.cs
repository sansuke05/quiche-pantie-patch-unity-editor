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

	public void SetURLFromFileName(string fileName) {
		www = new WWW(DREAMS_BASE_URL + fileName);
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

	public CommunicationState GetTexture(string fileName) {
		Texture2D tex;

		www.MoveNext();

		// リクエストが完了した時の処理
		if (www.isDone) {
			tex = www.texture;
			// テクスチャデータの保存
			creator = new FileCreator();
			creator.Create(fileName, tex);
			
			return CommunicationState.GETTING_DREAM_TEXTURE_FINISHED;
		}

		return CommunicationState.GETTING_DREAM_TEXTURE;
	}

	public float GetProgress() {
		return www.progress;
	}

	public void Clear() {
		www.Dispose();
	}
}

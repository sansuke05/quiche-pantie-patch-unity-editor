using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Communication {
	private const string URL = "";

	FileCreator creator;

	IEnumerator GetTexture() {
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		} else {
			// テクスチャデータを保存
			Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
			creator = new FileCreator();
			creator.Create(tex);
		}
	}
}

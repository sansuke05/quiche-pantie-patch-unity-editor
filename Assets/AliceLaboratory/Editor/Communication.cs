using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Communication {
	private const string URL = "http://www.alcot.biz/product/cld/img/ch/anz_up.png";

	FileCreator creator;

	public IEnumerator GetTexture() {
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
		yield return www.SendWebRequest();

		// リクエストが完了するまで待機
		while(!www.isDone) {
			yield return 0;
		}

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

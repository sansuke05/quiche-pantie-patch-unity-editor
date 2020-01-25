using UnityEngine;
using System.Collections;

public class NetworkTest: MonoBehaviour {
    void Start() {
            string url = "https://labten.net/pantie-patch/api/convert/mishe/0267.png";
            // コルーチンを実行
            StartCoroutine(GetPantie(url));
    }

    IEnumerator GetPantie(string url) {
        // wwwクラスのコンストラクタにパンツのURLを指定
        WWW www = new WWW(url);

        // パンツダウンロード完了を待機
        yield return www;

        // パンツ画像を取得
        Texture2D tex = www.texture;
    }
}
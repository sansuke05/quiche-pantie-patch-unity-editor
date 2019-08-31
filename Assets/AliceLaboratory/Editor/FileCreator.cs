using UnityEngine;
using UnityEditor;

public class FileCreator {

    private const string ASSET_PATH = "Assets/ExampleAssets/Materials/CreatedExample.mat";
    //private const string ASSET_PATH = "Assets/ExampleAssets/sample.png";

    public void Create(Texture2D _texture) {
        // 新しくマテリアルを生成
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.SetTexture("_MainTex", _texture);
        AssetDatabase.CreateAsset(mat, ASSET_PATH);
    }
}
using UnityEngine;
using UnityEditor;

public class FileCreator {

    private const string ASSET_PATH = "Assets/ExampleAssets/Materials/CreatedExample.mat";

    public void Create(Texture _texture) {
        // 新しくマテリアルを生成
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.SetTexture("_MainTex", _texture);
        UnityEditor.AssetDatabase.CreateAsset(mat, ASSET_PATH);
    }
}
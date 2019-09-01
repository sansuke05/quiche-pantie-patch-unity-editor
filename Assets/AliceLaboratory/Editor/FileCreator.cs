using UnityEngine;
using UnityEditor;

public class FileCreator {

    private const string ASSET_PATH = "Assets/AliceLaboratory/Dream/Textures";
    //private const string ASSET_PATH = "Assets/ExampleAssets/sample.png";

    public void Create(string fileName, Texture2D _texture) {
        // 新しくテクスチャを保存
        
        
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.SetTexture("_MainTex", _texture);
        AssetDatabase.CreateAsset(mat, ASSET_PATH);
    }
}
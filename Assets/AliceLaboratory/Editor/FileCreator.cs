using UnityEngine;
using UnityEditor;
using System.IO;

public class FileCreator {

    private const string ASSET_PATH = "Assets/AliceLaboratory/Dreams/";
    //private const string ASSET_PATH = "Assets/ExampleAssets/sample.png";

    public void Create(string fileName, Texture2D _texture) {
        string dir = Path.GetDirectoryName(ASSET_PATH);
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        
        // 新しくテクスチャを保存
        var png = _texture.EncodeToPNG();
        File.WriteAllBytes(ASSET_PATH + fileName, png);
    }
}
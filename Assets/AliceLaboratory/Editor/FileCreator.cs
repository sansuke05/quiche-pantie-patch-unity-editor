using UnityEngine;
using UnityEditor;
using System.IO;

public class FileCreator {

    private const string ASSET_DIR_PATH = "Assets/AliceLaboratory/Dreams/";
    //private const string ASSET_PATH = "Assets/ExampleAssets/sample.png";

    public void Create(string fileName, Texture2D _texture) {
        var dir = Path.GetDirectoryName(ASSET_DIR_PATH);
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
            AssetDatabase.ImportAsset(dir);
        }
        
        // 新しくテクスチャを保存
        var png = _texture.EncodeToPNG();
        var filePath = ASSET_DIR_PATH + fileName;
        File.WriteAllBytes(filePath, png);
        AssetDatabase.ImportAsset(filePath);
    }
}
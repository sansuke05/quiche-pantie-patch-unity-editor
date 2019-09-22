using System.IO;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class FileCreator {

        private const string ASSET_DIR_PATH = "Assets/AliceLaboratory/";
        //private const string CONVERTED_TEX_DIR_PATH = "Assets/ExampleAssets/sample.png";

        public void Create(string fileName, string parentDir, Texture2D _texture) {
            var assetPath = ASSET_DIR_PATH + parentDir + "/";
            var absPath = Path.GetDirectoryName(assetPath);
            if (!Directory.Exists(absPath)) {
                Directory.CreateDirectory(absPath);
                AssetDatabase.ImportAsset(absPath);
            }
        
            // 新しくテクスチャを保存
            var png = _texture.EncodeToPNG();
            var filePath = assetPath + fileName;
            File.WriteAllBytes(filePath, png);
            AssetDatabase.ImportAsset(filePath);
        }
    }
}
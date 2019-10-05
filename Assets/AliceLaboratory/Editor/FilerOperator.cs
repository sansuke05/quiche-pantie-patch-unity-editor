using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class FilerOperator {

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


        public List<string> getExistsTextures(string parentDir = "Dreams") {
            string[] filePathArray;
            var fileNames = new List<string>();

            if (!Directory.Exists(ASSET_DIR_PATH + parentDir)) {
                return null;
            }
            
            filePathArray = Directory.GetFiles(ASSET_DIR_PATH + parentDir, "*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)).ToArray();

            foreach (var filePath in filePathArray) {
                var file = Path.GetFileName(filePath);
                fileNames.Add(file);
            }
            
            return fileNames;
        }
    }
}
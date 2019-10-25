using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AliceLaboratory.Editor;

public class TextureCombinator : EditorWindow {

    public enum ColorChannel
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Alpha = 3
    }
    
    private const int TEXTURE_FIELD_SIZE = 64;
    
    private List<Texture2D> _textures;
    private List<ColorChannel> _channels;
    private int _width = 128;
    private int _height = 128;
    
    [MenuItem("Window/Texture Combinator")]
    public static void Open()
    {
        GetWindow<TextureCombinator>("Texture Combinator");
    }

    private void Awake()
    {
        _textures = new List<Texture2D>();
        _channels = new List<ColorChannel>();
        for (int i = 0; i < 4; i++) {
            _textures.Add(Texture2D.blackTexture);
            _channels.Add((ColorChannel)i);
        }
    }

    private void OnGUI()
    {
        // GUIを適当に描画する
        _width = EditorGUILayout.IntField("Width", _width);
        _height = EditorGUILayout.IntField("Height", _height);
        EditorGUILayout.LabelField("Textures");
        using (new EditorGUILayout.HorizontalScope("box")) {
            for (int i = 0; i < 4; i++) {
                using (new EditorGUILayout.VerticalScope()) {
                    EditorGUILayout.LabelField(((ColorChannel)i).ToString() + " is", GUILayout.Width(TEXTURE_FIELD_SIZE));
                    using (new EditorGUILayout.HorizontalScope()) {
                        _channels[i] = (ColorChannel)EditorGUILayout.EnumPopup(_channels[i], GUILayout.Width(TEXTURE_FIELD_SIZE * 0.7f));
                        EditorGUILayout.LabelField("of", GUILayout.Width(TEXTURE_FIELD_SIZE * 0.3f));
                    }
                    _textures[i] = EditorGUILayout.ObjectField(_textures[i], typeof(Texture2D), false, GUILayout.Width(TEXTURE_FIELD_SIZE), GUILayout.Height(TEXTURE_FIELD_SIZE)) as Texture2D;
                }
            }
        }

        if (GUILayout.Button("生成")) {
            var overTexPath = AssetDatabase.GetAssetPath(_textures[0]);
            var baseTexPath = AssetDatabase.GetAssetPath(_textures[1]);
            var overTex = FilerOperator.GetTexture(overTexPath);
            var baseTex = FilerOperator.GetTexture(baseTexPath);

            var output = Utilities.Overlap(overTex, baseTex);
            
            // 出来たテクスチャを保存する
            var filePath = EditorUtility.SaveFilePanel("Save Texture", "Assets", "name", "png");
            if (!string.IsNullOrEmpty(filePath)) {
                SaveTexture(filePath, output);
            }
        }
    }

    /// <summary>
    /// テクスチャを保存する
    /// </summary>
    private void SaveTexture(string filePath, Texture2D texture)
    {
        var bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// RenderTextureからピクセル情報を取得する
    /// </summary>
    private Color[] GetPixelsFromRT(RenderTexture target)
    {
        var preRT = RenderTexture.active;
        RenderTexture.active = target;
        
        // ReadPixels()でレンダーターゲットからテクスチャ情報を生成する
        var texture = new Texture2D(target.width, target.height);
        texture.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
        texture.Apply();
        
        RenderTexture.active = preRT;
        return texture.GetPixels();
    }
}
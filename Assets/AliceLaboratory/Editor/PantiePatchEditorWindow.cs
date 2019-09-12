using System;
using System.Collections;
using AliceLaboratory.Editor;
using UnityEngine;
using UnityEditor;


public class PantiePatchEditorWindow : EditorWindow {
    
    private Texture2D _tex;

    private Communication _com;

    /// <summary>
    /// Initialization
    /// </summary>
    [MenuItem("Editor/PantiePatch")]
    private static void Init() {
        var w = GetWindow<PantiePatchEditorWindow>();
        w.titleContent = new GUIContent("パンツパッチ");
        w.Show();
    }
    
    #region Unity Method

    private void OnEnable() {
        EditorApplication.update += OnUpdate;
    }

    private void OnDisable() {
        EditorApplication.update -= OnUpdate;
        EditorUtility.ClearProgressBar();
        Clear();
    }

    /// <summary>
    /// GUI setting
    /// </summary>
    private void OnGUI() {
        using(new GUILayout.VerticalScope()) {
            if(GUILayout.Button("パンツ一括ダウンロード")) {
                _com = new Communication();
            }
        }
        
        if (_tex != null) {
            EditorGUI.DrawTextureTransparent(new Rect(0, 70f, _tex.width * 0.5f, _tex.height * 0.5f), _tex);
        }
    }
    
    #endregion

    void OnUpdate() {
        if (_com != null) {
            Download();
        }
    }

    private void Download() {
        EditorUtility.DisplayProgressBar("Downloading...", "Downloading our dreams", _com.GetProgress());
        var status = _com.GetTexture();
        if (status == "finished") {
            _com = null;
            EditorUtility.ClearProgressBar();
        }
    }

    private void Clear() {
        if (_com != null) {
            _com.Clear();
        }

        _com = null;
    }
}
using System;
using System.Collections;
using AliceLaboratory.Editor;
using UnityEngine;
using UnityEditor;


public class PantiePatchEditorWindow : EditorWindow {

    private static IEnumerator _iEnumerator = null;

    private Texture2D _tex;

    /// <summary>
    /// Initialization
    /// </summary>
    [MenuItem("Editor/PantiePatch")]
    private static void Create() {
        GetWindow<PantiePatchEditorWindow>("パンツパッチ");
    }
    
    #region Unity Method

    private void OnEnable() {
        EditorApplication.update += Update;
    }

    private void OnDisable() {
        EditorApplication.update -= Update;
        Clear();
    }

    /// <summary>
    /// GUI setting
    /// </summary>
    private void OnGUI() {
        using(new GUILayout.VerticalScope()) {
            if(GUILayout.Button("パンツ一括ダウンロード")) {
                Download();
            }
        }
        
        if (_tex != null) {
            EditorGUI.DrawTextureTransparent(new Rect(0, 70f, _tex.width * 0.5f, _tex.height * 0.5f), _tex);
        }
    }
    
    #endregion

    private void Update() {
        if (_iEnumerator != null) {
            _iEnumerator.MoveNext();
        }
    }

    private void Download() {
        Debug.Log("Start coRoutine");
        var com = new Communication();
        // コルーチンの作成
        _iEnumerator = com.GetTexture(FinishDownload);
        //var test = new Test();
        //_iEnumerator = test.DelayLog();
    }

    private void FinishDownload(Texture2D texture) {
        _tex = texture;
        _iEnumerator = null;
    }

    private void Clear() {
        if (_tex != null) {
            DestroyImmediate(_tex);
        }
    }
}
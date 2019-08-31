using System.Collections;
using UnityEngine;
using UnityEditor;


public class PantiePatchEditorWindow : EditorWindow {

    private static IEnumerator _iEnumerator = null;

    /// <summary>
    /// Initialization
    /// </summary>
    [MenuItem("Editor/PantiePatch")]
    private static void Create() {
        GetWindow<PantiePatchEditorWindow>("パンツパッチ");
    }

    private void OnEnable() {
        EditorApplication.update += Update;
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
    }

    private void Update() {
        if (_iEnumerator != null) {
            while (_iEnumerator.MoveNext()) { Debug.Log("Running coRoutine..."); }
        }
    }

    private void Download() {
        Debug.Log("Start coRoutine");
        var com = new Communication();
        // コルーチンの作成
        _iEnumerator = com.GetTexture(FinishDownload);
    }

    private void FinishDownload() {
        _iEnumerator = null;
    }
}
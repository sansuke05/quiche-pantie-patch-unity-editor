using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;


public class PantiePatchEditorWindow : EditorWindow {

    private static IEnumerator _iEnumerator = null;

    private void OnGUI() {
        using(new GUILayout.VerticalScope()) {
            if(GUILayout.Button("パンツ一括ダウンロード")) {
                Download();
            }
        }
    }

    [MenuItem("Editor/PantiePatch")]
    private static void Create() {
        GetWindow<PantiePatchEditorWindow>("パンツパッチ");
    }

    private void Download() {
        Communication com = new Communication();
        _iEnumerator = com.GetTexture();
        
        if(_iEnumerator != null) {
            while (_iEnumerator.MoveNext()) {}
        }
    }
}
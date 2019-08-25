using UnityEngine;
using UnityEditor;


public class PantiePatchEditorWindow : EditorWindow {

    private Material mat = null;

    private void OnGUI() {
        using(new GUILayout.VerticalScope()) {
            if(GUILayout.Button("パンツ一括ダウンロード")) {
                //pass
            }
        }
        using(new GUILayout.HorizontalScope()) {
            mat = EditorGUILayout.ObjectField("Sample", mat, typeof(Material), false) as Material;
        }
    }

    [MenuItem("Editor/PantiePatch")]
    private static void Create() {
        GetWindow<PantiePatchEditorWindow>("パンツパッチ");
    }
}
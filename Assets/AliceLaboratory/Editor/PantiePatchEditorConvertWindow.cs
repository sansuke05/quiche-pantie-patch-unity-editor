using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorConvertWindow : EditorWindow {

        private Texture convertTexture;

        private static string[,] models = {
            {"吸血鬼アンナちゃん", "anna"},
            {"吸血鬼アンナちゃん(ライト)", "anna_light"},
            {"ファジーちゃん", "fuzzy"},
            {"リンツちゃん", "linz"},
            {"リンツちゃん(素体)", "linz_nbody"},
            {"ルアちゃん", "lua"},
            {"ルアちゃん(クエスト)", "lua_quest"},
            {"ミルクちゃん", "milk"},
            {"ミーシェちゃん", "mishe"},
            {"キッシュちゃん", "quiche"},
            {"キッシュちゃん(ブラ)", "quiche_bra"},
            {"キッシュちゃん(ライト)", "quiche_light"},
            {"キッシュちゃん(素体)", "quiche_nbody"},
            {"ラムネちゃん", "ramne"},
            {"シャーロちゃん", "shaclo"},
            {"たぬちゃん", "tanu"},
            {"右近ちゃん", "ukon"},
            {"幽狐さん", "yuko"},
            {"VRoidちゃん", "vroid"},
            {"コルネットちゃん", "cornet"},
            {"コルネットちゃん(素体)", "cornet_nbody"}
        };

        private int selectedIndex = -1;

        private static string[] modelNames;

        [MenuItem("Editor/PantiePatch/パンツ変換")]
        private static void Init() {
            modelNames = new string[models.GetLength(0)];
            for (int i = 0; i < models.GetLength(0); i++) {
                modelNames[i] = models[i, 0];
            }
            
            var window = GetWindow<PantiePatchEditorConvertWindow>();
            window.titleContent = new GUIContent("パンツ変換");
            window.Show();
        }

        private void OnGUI() {
            using (new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("変換するパンツを選択");
                var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                convertTexture = EditorGUILayout.ObjectField(convertTexture, typeof(Texture), false, option) as Texture;
                
                EditorGUILayout.LabelField("変換対象のアバター");
            }
            selectedIndex = GUILayout.SelectionGrid(selectedIndex, modelNames, 2);
            using (new GUILayout.VerticalScope()) {
                GUILayout.Space(20);
                if(GUILayout.Button("変換")) {
                    
                }
            }
        }
    }
}
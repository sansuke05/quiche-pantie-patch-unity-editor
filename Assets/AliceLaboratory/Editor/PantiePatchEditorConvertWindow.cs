using System;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorConvertWindow : EditorWindow {

        private Gateway _gate;
        
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

        /// <summary>
        /// Initialization
        /// </summary>
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
            using (new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("変換するパンツを選択");
                var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                convertTexture = EditorGUILayout.ObjectField(convertTexture, typeof(Texture), false, option) as Texture;
                
                EditorGUILayout.LabelField("変換対象のアバター");
            }
            selectedIndex = GUILayout.SelectionGrid(selectedIndex, modelNames, 2);
            using (new GUILayout.VerticalScope()) {
                GUILayout.Space(20);
                bool isDisable = convertTexture == null || selectedIndex < 0;
                EditorGUI.BeginDisabledGroup(isDisable);
                if(GUILayout.Button("変換")) {
                    _gate = new Gateway(convertTexture.name + ".png", models[selectedIndex, 1]);
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        
        #endregion

        void OnUpdate() {
            if (_gate != null) {
                Convert();
            }
        }

        private void Convert() {
            EditorUtility.DisplayProgressBar("Converting", "Your dream come true soon...", _gate.GetProgress());
            if (_gate.GetConvertedTexture(convertTexture.name + ".png", models[selectedIndex, 1]) == GatewayState.GETTING_CONVERTED_TEXTURE_COMPLETED) {
                _gate = null;
                EditorUtility.ClearProgressBar();
                Debug.Log("Converting completed!");
            }
        }
        
        private void Clear() {
            if (_gate != null) {
                _gate.Clear();
            }

            _gate = null;
        }
    }
}
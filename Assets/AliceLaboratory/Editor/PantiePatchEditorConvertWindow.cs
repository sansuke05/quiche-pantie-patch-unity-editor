using System;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorConvertWindow : EditorWindow {

        private Gateway _gate;
        
        private Texture convertTexture;

        private Texture baseAvaterTexture;
        
        //スクロール位置
        private Vector2 _scrollPosition = Vector2.zero;

        private bool[] _disable = {false, true};

        private int _disableMode = 0;

        private static AvatarsData _avatersData;

        private int selectedIndex = -1;

        /// <summary>
        /// Initialization
        /// </summary>
        [MenuItem("Editor/PantiePatch/パンツ変換")]
        private static void Init() {
            // ScriptableObjectからアバターデータを読み込む
            var file = new FilerOperator();
            _avatersData = file.readAvatersData();
            
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
                using (new GUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("変換するパンツを選択");
                    var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                    convertTexture = EditorGUILayout.ObjectField(convertTexture, typeof(Texture), false, option) as Texture;
                }
                using (new GUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("重ねるアバターのテクスチャを選択");
                    var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                    baseAvaterTexture = EditorGUILayout.ObjectField(baseAvaterTexture, typeof(Texture), false, option) as Texture;
                }
                EditorGUILayout.LabelField("変換対象のアバター");
            }
            if (_avatersData != null) {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                selectedIndex = GUILayout.SelectionGrid(selectedIndex, _avatersData.display_names, 2);
                EditorGUILayout.EndScrollView();
            } else {
                EditorGUILayout.HelpBox(
                    "アバターのデータがダウンロードされていません\nメニューのデータダウンロード > 対応アバター情報の更新からデータのダウンロードをして下さい",
                    MessageType.Info
                );
            }
            using (new GUILayout.VerticalScope()) {
                GUILayout.Space(20);
                _disable[0] = convertTexture == null || selectedIndex < 0;
                EditorGUI.BeginDisabledGroup(_disable[_disableMode]);
                if(GUILayout.Button("変換")) {
                    _gate = new Gateway(convertTexture.name + ".png", _avatersData.models[selectedIndex]);
                    _disableMode = 1;
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
            if (_gate.GetConvertedTexture(convertTexture.name + ".png", _avatersData.models[selectedIndex]) == GatewayState.GETTING_CONVERTED_TEXTURE_COMPLETED) {
                _gate = null;
                _disableMode = 0;
                EditorUtility.ClearProgressBar();
                Debug.Log("Converting completed!");
            }
        }
        
        private void Clear() {
            if (_gate != null) {
                _gate.Clear();
            }

            _disableMode = 0;
            _gate = null;
        }
    }
}
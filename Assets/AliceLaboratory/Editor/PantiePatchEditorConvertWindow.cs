using System;
using UnityEditor;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorConvertWindow : EditorWindow {

        private Gateway _gateway;
        
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
                    _gateway = new Gateway();
                    _disableMode = 1;
                    Convert().Forget();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        
        #endregion

        void OnUpdate() 
        {
            if (_gateway != null) {
                EditorUtility.DisplayProgressBar("Converting", "Your dream come true soon...", _gateway.GetProgress());
            }
        }

        private async UniTaskVoid Convert() 
        {
            var fileName = convertTexture.name + ".png";
            var modelName = _avatersData.models[selectedIndex];
            var tex = await _gateway.GetConvertedTexture(fileName, modelName, baseAvaterTexture);

            // 重ねるアバターのテクスチャが設定されていればテクスチャを合成する
            if (baseAvaterTexture != null)
            {
                // Pathからアバターのテクスチャを取得
                var baseTexPath = AssetDatabase.GetAssetPath(baseAvaterTexture);
                var baseTex2D = FilerOperator.GetTexture(baseTexPath);
                tex = TextureUtils.Overlap(overTex: tex, baseTex: baseTex2D);
            }

            // テクスチャデータの保存
            var dir = "ConvertedDreams/" + modelName;
            var creator = new FilerOperator();
            creator.Create(fileName, dir, tex);

            _gateway = null;
            _disableMode = 0;
            EditorUtility.ClearProgressBar();
            Debug.Log("Converting completed!");
        }
        
        private void Clear() {
            _disableMode = 0;
            _gateway = null;
        }
    }
}
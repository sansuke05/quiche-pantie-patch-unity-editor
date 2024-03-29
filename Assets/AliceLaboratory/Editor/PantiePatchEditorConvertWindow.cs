using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;

#else
using UnityEngine.Experimental.UIElements;
#endif

namespace AliceLaboratory.Editor
{
    public sealed class PantiePatchEditorConvertWindow : EditorWindow
    {
        AsyncReactiveProperty<Texture> BaseAvatarTexture { get; } = new AsyncReactiveProperty<Texture>(null);
        AsyncReactiveProperty<Texture> ConvertTexture { get; } = new AsyncReactiveProperty<Texture>(null);
        AsyncReactiveProperty<int> SelectedAvatarIndex { get; } = new AsyncReactiveProperty<int>(-1);
        AsyncReactiveProperty<bool> ConvertRunning { get; } = new AsyncReactiveProperty<bool>(false);
        ReadOnlyAsyncReactiveProperty<bool> CanStartConvert { get; }

        Vector2 _scrollPosition = Vector2.zero;
        AvatarsData _avatarsData;

        public PantiePatchEditorConvertWindow()
        {
            CanStartConvert = UniTaskAsyncEnumerable.CombineLatest(ConvertTexture, SelectedAvatarIndex, ConvertRunning,
                    (convertTexture, selectedAvatarIndex, convertRunning) =>
                        convertTexture != null && selectedAvatarIndex != -1 && !convertRunning)
                .ToReadOnlyAsyncReactiveProperty(default);
        }

        [MenuItem("Editor/PantiePatch/パンツ変換")]
        static void Initialize()
        {
            var window = GetWindow<PantiePatchEditorConvertWindow>();
            window.titleContent = new GUIContent("パンツ変換");
        }

        void OnEnable()
        {
            // ScriptableObjectからアバターデータを読み込む
            var file = new FilerOperator();
            _avatarsData = file.readAvatersData();
        }

#if !UNITY_2019_4_OR_NEWER
        /// <summary>
        /// GUI setting
        /// </summary>
        private void OnGUI() {
            using (new GUILayout.VerticalScope()) {
                using (new GUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("変換するパンツを選択");
                    var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                    ConvertTexture.Value =
 EditorGUILayout.ObjectField(ConvertTexture.Value, typeof(Texture), false, option) as Texture;
                }
                using (new GUILayout.HorizontalScope()) {
                    EditorGUILayout.LabelField("重ねるアバターのテクスチャを選択");
                    var option = new []{GUILayout.Width (64), GUILayout.Height (64)};
                    BaseAvatarTexture.Value =
 EditorGUILayout.ObjectField(BaseAvatarTexture.Value, typeof(Texture), false, option) as Texture;
                }
                EditorGUILayout.LabelField("変換対象のアバター");
            }
            if (_avatarsData != null) {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                SelectedAvatarIndex.Value =
 GUILayout.SelectionGrid(SelectedAvatarIndex.Value, _avatarsData.display_names, 2);
                EditorGUILayout.EndScrollView();
            } else {
                EditorGUILayout.HelpBox(
                    "アバターのデータがダウンロードされていません\nメニューのデータダウンロード > 対応アバター情報の更新からデータのダウンロードをして下さい",
                    MessageType.Info
                );
            }
            using (new GUILayout.VerticalScope()) {
                GUILayout.Space(20);
                EditorGUI.BeginDisabledGroup(!CanStartConvert.Value);
                if(GUILayout.Button("変換")) {
                    Convert(ConvertTexture.Value, BaseAvatarTexture.Value, SelectedModelName);
                }
                EditorGUI.EndDisabledGroup();
            }
        }
#else
        void CreateGUI()
        {
            const string layoutDirPath = "Assets/AliceLaboratory/Editor/Layout";
            var root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{layoutDirPath}/PantiePatchEditorConvertWindow.uxml");
            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                $"{layoutDirPath}/PantiePatchEditorConvertWindow.uss");
            root.styleSheets.Add(style);
            visualTree.CloneTree(root);

            var convertTextureSelector = root.Q<ObjectField>("ConvertTextureSelector");
            convertTextureSelector.objectType = typeof(Texture);
            convertTextureSelector.RegisterValueChangedCallback(change =>
                ConvertTexture.Value = (Texture) change.newValue);

            var baseAvatarTextureSelector = root.Q<ObjectField>("BaseAvatarTextureSelector");
            baseAvatarTextureSelector.objectType = typeof(Texture);
            baseAvatarTextureSelector.RegisterValueChangedCallback(change =>
                BaseAvatarTexture.Value = (Texture) change.newValue);

            var avatarsView = root.Q<IMGUIContainer>();
            avatarsView.onGUIHandler = () =>
            {
                if (_avatarsData != null)
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    SelectedAvatarIndex.Value =
                        GUILayout.SelectionGrid(SelectedAvatarIndex.Value, _avatarsData.display_names, 2);
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.HelpBox(
                        "アバターのデータがダウンロードされていません\nメニューのデータダウンロード > 対応アバター情報の更新からデータのダウンロードをして下さい",
                        MessageType.Info
                    );
                }
            };

            var convertButton = root.Q<Button>();
            convertButton.SetEnabled(false);
            CanStartConvert.ForEachAsync(canStartConvert => convertButton.SetEnabled(canStartConvert));
            convertButton.clicked += () => Convert(ConvertTexture.Value, BaseAvatarTexture.Value, SelectedModelName);
        }
#endif
        string SelectedModelName => _avatarsData.models[SelectedAvatarIndex.Value];

        void Convert(Texture convertTexture, Texture baseAvatarTexture, string modelName) =>
            ConvertImpl(convertTexture, baseAvatarTexture, modelName).Forget();

        async UniTaskVoid ConvertImpl(Texture convertTexture, Texture baseAvatarTexture, string modelName)
        {
            ConvertRunning.Value = true;
            try
            {
                var fileName = convertTexture.name + ".png";
                var (task, request) = Gateway.GetConvertedTexture(fileName, modelName, baseAvatarTexture);
                Gateway.ShowProgressBarForUnityWebRequest(request, "Converting", "Your dream come true soon...");
                var tex = await task;
                if (tex is null)
                {
                    throw new UnityWebRequestException(request);
                }

                // 重ねるアバターのテクスチャが設定されていればテクスチャを合成する
                if (baseAvatarTexture != null)
                {
                    // Pathからアバターのテクスチャを取得
                    var baseTexPath = AssetDatabase.GetAssetPath(baseAvatarTexture);
                    var baseTex2D = FilerOperator.GetTexture(baseTexPath);
                    tex = TextureUtils.Overlap(overTex: tex, baseTex: baseTex2D);
                }

                // テクスチャデータの保存
                var dir = "ConvertedDreams/" + modelName;
                var creator = new FilerOperator();
                creator.Create(fileName, dir, tex);
                Debug.Log("Converting completed!");
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError("Download Error: パンツの変換に失敗しました");
            }
            finally
            {
                ConvertRunning.Value = false;
            }
        }
    }
}
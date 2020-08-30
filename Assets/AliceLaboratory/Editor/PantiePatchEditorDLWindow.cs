using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorDLWindow : EditorWindow {
    
        private Gateway _gateway;

        private GUIFlagState _state = GUIFlagState.NONE;

        private bool _guiDisable = false;

        /// <summary>
        /// Initialization
        /// </summary>
        [MenuItem("Editor/PantiePatch/データダウンロード")]
        private static void Init() {
            var w = GetWindow<PantiePatchEditorDLWindow>();
            w.titleContent = new GUIContent("Download");
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
                EditorGUILayout.LabelField("変換元パンツテクスチャダウンロード");
                EditorGUI.BeginDisabledGroup(_guiDisable);
                if(GUILayout.Button("ダウンロード")) {
                    _gateway = new Gateway();
                    _state = GUIFlagState.DOWNLOADING_DREAMS;
                    _guiDisable = true;
                    DownloadDreams().Forget();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();

            using(new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("対応アバター情報の更新");
                EditorGUI.BeginDisabledGroup(_guiDisable);
                if (GUILayout.Button("更新")) {
                    _gateway = new Gateway();
                    _state = GUIFlagState.UPDATING_AVATERS_DATA;
                    _guiDisable = true;
                    UpdateAvaters().Forget();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    
        #endregion

        void OnUpdate() {
            if (_gateway == null) {
                return;
            }

            if (_state == GUIFlagState.DOWNLOADING_DREAMS) 
            {
                EditorUtility.DisplayProgressBar("Downloading...", "Downloading our dreams", _gateway.GetProgress());
            } 
            else if (_state == GUIFlagState.UPDATING_AVATERS_DATA)
            {
                EditorUtility.DisplayProgressBar("Updating...", "Updating avaters data", _gateway.GetProgress());
            }
        }

        private async UniTaskVoid DownloadDreams()
        {
            // --- 変換元パンツテクスチャのリストをDL ---
            var dreamsData = await _gateway.GetDreamsData();
            if (dreamsData == null)
            {
                Debug.LogError("Download Error: 変換元パンツ情報のダウンロードに失敗しました");
                ClearGUIWaiting();
                return;
            }

            // --- 変換元テクスチャを一括ダウンロード&保存 ---
            var existFiles = FilerOperator.getExistsTextures();

            foreach(var imageName in dreamsData.images)
            {
                // 既にローカルにテクスチャが存在する場合はスキップ
                if (existFiles != null && existFiles.Contains(imageName))
                {
                    continue;
                }

                var tex = await _gateway.GetDreamTexture(imageName);
                
                // テクスチャデータの保存
                var creator = new FilerOperator();
                creator.Create(imageName, "Dreams", tex);
            }

            ClearGUIWaiting();
        }

        private async UniTaskVoid UpdateAvaters() 
        {
            var data = await _gateway.GetAvatarsData();
            if(data != null) {
                var file = new FilerOperator();
                file.SaveAvatarsData(data);

                ClearGUIWaiting();
                Debug.Log("Updating completed!");
                Debug.Log(string.Join(",",data.display_names));
                Debug.Log(string.Join(",",data.models));
            }
        }

        private void ClearGUIWaiting()
        {
            _gateway = null;
            _state = GUIFlagState.NONE;
            _guiDisable = false;
            EditorUtility.ClearProgressBar();
        }

        private void Clear() {
            _guiDisable = false;
            _gateway = null;
        }
    }
}
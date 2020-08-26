using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorDLWindow : EditorWindow {
    
        private Gateway _gateway;

        private GatewayOperator _operator;

        private GUIFlagState _state = GUIFlagState.NONE;

        private bool _processing;

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
                    _operator = new GatewayOperator();
                    _state = GUIFlagState.DOWNLOADING_DREAMS;
                    _guiDisable = true;
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();

            using(new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("対応アバター情報の更新");
                EditorGUI.BeginDisabledGroup(_guiDisable);
                if (GUILayout.Button("更新")) {
                    _gateway = new Gateway("GetAvatarsData");
                    _state = GUIFlagState.UPDATING_AVATERS_DATA;
                    _guiDisable = true;
                    UpdateAvaters();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    
        #endregion

        void OnUpdate() {
            if (_gateway == null) {
                return;
            }

            if (_state == GUIFlagState.DOWNLOADING_DREAMS) {
                Download();
            }
        }

        private void Download() {
            EditorUtility.DisplayProgressBar("Downloading...", "Downloading our dreams", _gateway.GetProgress());
            if (!_processing) {
                _operator.State = GatewayState.GETTING_DREAMS_LIST;
                _processing = true;
            }
            _operator.Execute(_gateway);
        
            if (_operator.State == GatewayState.GETTING_DREAM_TEXTURES_COMPLETED) {
                _processing = false;
                _gateway = null;
                _state = GUIFlagState.NONE;
                _guiDisable = false;
                EditorUtility.ClearProgressBar();
                Debug.Log("Downloading completed!");
            }
        }

        private async void UpdateAvaters() 
        {
            EditorUtility.DisplayProgressBar("Updating...", "Updating avaters data", _gateway.GetProgress());
            var data = await _gateway.GetAvatarsData();
            if(data != null) {
                var file = new FilerOperator();
                file.SaveAvatarsData(data);

                _gateway = null;
                _state = GUIFlagState.NONE;
                _guiDisable = false;
                EditorUtility.ClearProgressBar();
                Debug.Log("Updating completed!");
                Debug.Log(string.Join(",",data.display_names));
                Debug.Log(string.Join(",",data.models));
            }
        }

        private void Clear() {
            if (_gateway != null) {
                _gateway.Clear();
            }

            _guiDisable = false;
            _gateway = null;
        }
    }
}
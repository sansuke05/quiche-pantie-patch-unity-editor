using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorDLWindow : EditorWindow {
    
        private Gateway _gate;

        private GatewayOperator _operator;

        private GUIFlagState _state = GUIFlagState.NONE;

        private bool _processing;

        private bool _disable = false;

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
                EditorGUI.BeginDisabledGroup(_disable);
                if(GUILayout.Button("ダウンロード")) {
                    _gate = new Gateway();
                    _operator = new GatewayOperator();
                    _state = GUIFlagState.DOWNLOADING_DREAMS;
                    _disable = true;
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();

            using(new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("対応アバター情報の更新");
                EditorGUI.BeginDisabledGroup(_disable);
                if (GUILayout.Button("更新")) {

                }
                EditorGUI.EndDisabledGroup();
            }
        }
    
        #endregion

        void OnUpdate() {
            if (_gate == null) {
                return;
            }

            if (_state == GUIFlagState.DOWNLOADING_DREAMS) {
                Download();
            } else if(_state == GUIFlagState.UPDATING_AVATERS_DATA) {
                UpdateAvaters();
            }
        }

        private void Download() {
            EditorUtility.DisplayProgressBar("Downloading...", "Downloading our dreams", _gate.GetProgress());
            if (!_processing) {
                _operator.State = GatewayState.GETTING_DREAMS_LIST;
                _processing = true;
            }
            _operator.Execute(_gate);
        
            if (_operator.State == GatewayState.GETTING_DREAM_TEXTURES_COMPLETED) {
                _processing = false;
                _gate = null;
                _state = GUIFlagState.NONE;
                _disable = false;
                EditorUtility.ClearProgressBar();
                Debug.Log("Downloading completed!");
            }
        }

        private void UpdateAvaters() {
            EditorUtility.DisplayProgressBar("Updating...", "Updating avaters data", _gate.GetProgress());
            // ここから

        }

        private void Clear() {
            if (_gate != null) {
                _gate.Clear();
            }

            _disable = false;
            _gate = null;
        }
    }
}
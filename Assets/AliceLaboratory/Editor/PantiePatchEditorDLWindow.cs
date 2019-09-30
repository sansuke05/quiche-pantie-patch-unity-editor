using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorDLWindow : EditorWindow {
    
        private Gateway _gate;

        private GatewayOperator _operator;

        private bool _processing;

        private bool _disable = false;

        /// <summary>
        /// Initialization
        /// </summary>
        [MenuItem("Editor/PantiePatch/パンツデータダウンロード")]
        private static void Init() {
            var w = GetWindow<PantiePatchEditorDLWindow>();
            w.titleContent = new GUIContent("PantieDL");
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
                EditorGUI.BeginDisabledGroup(_disable);
                if(GUILayout.Button("パンツデータダウンロード")) {
                    _gate = new Gateway();
                    _operator = new GatewayOperator();
                    _disable = true;
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    
        #endregion

        void OnUpdate() {
            if (_gate != null) {
                Download();
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
                _disable = false;
                EditorUtility.ClearProgressBar();
                Debug.Log("Downloading completed!");
            }
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
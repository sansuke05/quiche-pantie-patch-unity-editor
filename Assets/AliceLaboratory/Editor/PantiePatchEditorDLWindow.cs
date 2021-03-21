using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class PantiePatchEditorDLWindow : EditorWindow {

        AsyncReactiveProperty<bool> DownloadingDreams { get; } = new AsyncReactiveProperty<bool>(false);
        AsyncReactiveProperty<bool> UpdatingAvatars { get; } = new AsyncReactiveProperty<bool>(false);
        ReadOnlyAsyncReactiveProperty<bool> GUIDisable { get; }

        public PantiePatchEditorDLWindow()
        {
            GUIDisable = UniTaskAsyncEnumerable.CombineLatest(DownloadingDreams, UpdatingAvatars, (a, b) => a | b).ToReadOnlyAsyncReactiveProperty(default);
        }
        /// <summary>
        /// Initialization
        /// </summary>
        [MenuItem("Editor/PantiePatch/データダウンロード")]
        private static void Init() {
            GetWindow<PantiePatchEditorDLWindow>("Download").Show();
        }

        #region Unity Method

        /// <summary>
        /// GUI setting
        /// </summary>
        private void OnGUI() {
            using(new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("変換元パンツテクスチャダウンロード");
                EditorGUI.BeginDisabledGroup(GUIDisable.Value);
                if(GUILayout.Button("ダウンロード")) {
                    DownloadDreams().Forget();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();

            using(new GUILayout.VerticalScope()) {
                EditorGUILayout.LabelField("対応アバター情報の更新");
                EditorGUI.BeginDisabledGroup(GUIDisable.Value);
                if (GUILayout.Button("更新")) {
                    UpdateAvatars().Forget();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
    
        #endregion


        private async UniTaskVoid DownloadDreams()
        {
            DownloadingDreams.Value = true;

            // --- 変換元パンツテクスチャのリストをDL ---
            Dream dreamsData = new Dream();
            try
            {
                var getDreamsData = Gateway.GetDreamsData();
                Gateway.ShowProgressBarForUnityWebRequest(getDreamsData.Request, "Downloading...", "Downloading our dreams");
                dreamsData = await getDreamsData.Task;
                if (dreamsData is null)
                {
                    throw new UnityWebRequestException(getDreamsData.Request);
                }
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError("Download Error: 変換元パンツ情報のダウンロードに失敗しました");
                DownloadingDreams.Value = false;
            }

            // --- 変換元テクスチャを一括ダウンロード&保存 ---
            try
            {
                var existFiles = FilerOperator.getExistsTextures();

                foreach (var imageName in dreamsData.images)
                {
                    // 既にローカルにテクスチャが存在する場合はスキップ
                    if (existFiles != null && existFiles.Contains(imageName))
                    {
                        continue;
                    }

                    var getDreamTexture = Gateway.GetDreamTexture(imageName);
                    Gateway.ShowProgressBarForUnityWebRequest(getDreamTexture.Request, "Downloading...",
                        "Downloading our dreams");
                    var tex = await getDreamTexture.Task;

                    // テクスチャデータの保存
                    var creator = new FilerOperator();
                    creator.Create(imageName, "Dreams", tex);
                }
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError("Download Error: 変換元パンツテクスチャのダウンロードに失敗しました");
            }
            finally
            {
                DownloadingDreams.Value = false;
            }
        }

        private async UniTaskVoid UpdateAvatars() 
        {
            UpdatingAvatars.Value = true;
            try
            {
                var getAvatarsData = Gateway.GetAvatarsData();
                Gateway.ShowProgressBarForUnityWebRequest(getAvatarsData.Request, "Updating...", "Updating avatars data");
                var data = await getAvatarsData.Task;
                if(data != null) {
                    var file = new FilerOperator();
                    file.SaveAvatarsData(data);

                    Debug.Log("Updating completed!");
                    Debug.Log(string.Join(",",data.display_names));
                    Debug.Log(string.Join(",",data.models));
                }
                else
                {
                    throw new UnityWebRequestException(getAvatarsData.Request);
                }
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError("Download Error: アバター情報のダウンロードに失敗しました");
            }
            finally
            {
                UpdatingAvatars.Value = false;
            }
        }
    }
}
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace AliceLaboratory.Editor {
	public static class Gateway {
		private const string DREAMS_BASE_URL = "https://labten.net/pantie-patch/api/dream/";
		private const string CONVERTING_BASE_URL = "https://labten.net/pantie-patch/api/convert/";

		
		public static (UniTask<Dream> Task,UnityWebRequest Request) GetDreamsData()
        {
			var request = UnityWebRequest.Get(DREAMS_BASE_URL);
			return (GetDreamsData(request), request);
		}

		static async UniTask<Dream> GetDreamsData(UnityWebRequest request) 
		{
			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return JsonUtility.FromJson<Dream>(request.downloadHandler.text);
		}

		public static (UniTask<AvatarsData> Task, UnityWebRequest Request) GetAvatarsData()
        {
			var request = UnityWebRequest.Get(CONVERTING_BASE_URL);
			return (GetAvatarsData(request),request);
		}

		static async UniTask<AvatarsData> GetAvatarsData(UnityWebRequest request)
        {

			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return JsonUtility.FromJson<AvatarsData>(request.downloadHandler.text);
        }

		public static (UniTask<Texture2D> Task, UnityWebRequest Request) GetDreamTexture(string fileName)
        {
			var request = UnityWebRequestTexture.GetTexture(DREAMS_BASE_URL + fileName);
			return (GetDreamTexture(request), request);
		}

		public static async UniTask<Texture2D> GetDreamTexture(UnityWebRequest request)
		{
			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
            {
				Debug.LogError(request.error);
				return null;
            }

			return DownloadHandlerTexture.GetContent(request);
		}

		public static (UniTask<Texture2D> Task, UnityWebRequest Request) GetConvertedTexture(string fileName, string modelName, Texture baseTex)
        {
			var request = UnityWebRequestTexture.GetTexture(CONVERTING_BASE_URL + modelName + "/" + fileName);
			return (GetConvertedTexture(request), request);
		}

		public static async UniTask<Texture2D> GetConvertedTexture(UnityWebRequest request) 
		{

			await request.SendWebRequest();

			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return DownloadHandlerTexture.GetContent(request);
		}

		public static void ShowProgressBarForUnityWebRequest(UnityWebRequest request, string title, string info)
        {
			ShowProgressBarForUnityWebRequestImpl(request, title, info).Forget();
        }

		static async UniTaskVoid ShowProgressBarForUnityWebRequestImpl(UnityWebRequest request,string title,string info)
        {
			while (!request.isDone)
			{
				EditorUtility.DisplayProgressBar(title, info, request.downloadProgress);
				await UniTask.Yield();
			}
			EditorUtility.ClearProgressBar();
		}
	}
}

using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace AliceLaboratory.Editor {
	public class Gateway {
		private const string DREAMS_BASE_URL = "https://labten.net/pantie-patch/api/dream/";
		private const string CONVERTING_BASE_URL = "https://labten.net/pantie-patch/api/convert/";

		private float _progress = 0.0f;

		async UniTaskVoid UpdateProgress(UnityWebRequest request)
        {
            while (!request.isDone)
            {
				_progress = request.downloadProgress;
				await UniTask.Yield();
			}
			_progress = request.downloadProgress;
        }
		
		public async UniTask<Dream> GetDreamsData() 
		{
			var request = UnityWebRequest.Get(DREAMS_BASE_URL);
			UpdateProgress(request).Forget();
			await request.SendWebRequest();


			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return JsonUtility.FromJson<Dream>(request.downloadHandler.text);
		}

		public async UniTask<AvatarsData> GetAvatarsData()
        {
			var request = UnityWebRequest.Get(CONVERTING_BASE_URL);
			UpdateProgress(request).Forget();
			await request.SendWebRequest();


			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return JsonUtility.FromJson<AvatarsData>(request.downloadHandler.text);
        }

		public async UniTask<Texture2D> GetDreamTexture(string fileName)
		{
			var request = UnityWebRequestTexture.GetTexture(DREAMS_BASE_URL + fileName);
			UpdateProgress(request).Forget();
			await request.SendWebRequest();


			if (request.isNetworkError || request.isHttpError)
            {
				Debug.LogError(request.error);
				return null;
            }

			return DownloadHandlerTexture.GetContent(request);
		}

		public async UniTask<Texture2D> GetConvertedTexture(string fileName, string modelName, Texture baseTex) 
		{
			var request = UnityWebRequestTexture.GetTexture(CONVERTING_BASE_URL + modelName + "/" + fileName);
			UpdateProgress(request).Forget();
			await request.SendWebRequest();


			if (request.isNetworkError || request.isHttpError)
			{
				Debug.LogError(request.error);
				return null;
			}

			return DownloadHandlerTexture.GetContent(request);
		}

		public float GetProgress() {
			return _progress;
		}
	}
}

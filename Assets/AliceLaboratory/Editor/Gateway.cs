using UnityEngine;

namespace AliceLaboratory.Editor {
	public class Gateway {
		//old URL
		//private const string DREAMS_BASE_URL = "http://pantie-patch.herokuapp.com/api/dream/";
		//private const string CONVERTING_BASE_URL = "http://pantie-patch.herokuapp.com/api/convert/";
		private const string DREAMS_BASE_URL = "https://labten.net/pantie-patch/api/dream/";
		private const string CONVERTING_BASE_URL = "https://labten.net/pantie-patch/api/convert/";
		
		private WWW www;
	
		FileCreator creator;

		public Gateway() {
			www = new WWW(DREAMS_BASE_URL);
		}

		public Gateway(string fileName, string modelName) {
			www = new WWW(CONVERTING_BASE_URL + modelName + "/" + fileName);
		}
		
		public void SetUrlFromFileName(string fileName) {
			www = new WWW(DREAMS_BASE_URL + fileName);
		}
		
		public Dream GetDreams() {
			Dream dream = null;
		
			www.MoveNext();
		
			// リクエストが完了した時の処理
			if (www.isDone) {
				dream = JsonUtility.FromJson<Dream>(www.text);
			}
		
			return dream;
		}

		public GatewayState GetTexture(string fileName) {
			Texture2D tex;

			www.MoveNext();

			// リクエストが完了した時の処理
			if (www.isDone) {
				tex = www.texture;
				// テクスチャデータの保存
				creator = new FileCreator();
				creator.Create(fileName, "Dreams", tex);
			
				return GatewayState.GETTING_DREAM_TEXTURE_FINISHED;
			}

			return GatewayState.GETTING_DREAM_TEXTURE;
		}

		public GatewayState GetConvertedTexture(string fileName, string modelName) {
			Texture2D tex;

			www.MoveNext();
			
			// リクエストが完了した時の処理
			if (www.isDone) {
				tex = www.texture;
				var dir = "ConvertedDreams/" + modelName;
				// テクスチャデータの保存
				creator = new FileCreator();
				creator.Create(fileName, dir, tex);
			
				return GatewayState.GETTING_CONVERTED_TEXTURE_COMPLETED;
			}
			
			return GatewayState.GETTING_CONVERTED_TEXTURE;
		}

		public float GetProgress() {
			return www.progress;
		}

		public void Clear() {
			www.Dispose();
		}
	}
}

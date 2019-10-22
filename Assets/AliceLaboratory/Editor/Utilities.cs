using UnityEngine;

namespace AliceLaboratory.Editor {
    public class Utilities {

        /// <summary>
        /// テクスチャの重ね合わせ処理
        /// </summary>
        public static Texture2D Overlap(Texture2D overTex, Texture2D baseTex) {
            var output = new Texture2D(baseTex.width, baseTex.height);
            var dest = output.GetPixels();
            var renderTexture = RenderTexture.GetTemporary(baseTex.width, baseTex.height);

            Graphics.Blit(baseTex, renderTexture);
            dest = GetPixelsFromRT(renderTexture);
            Graphics.Blit(overTex, renderTexture);
            var pixels = GetPixelsFromRT(renderTexture);
            for (int i = 0; i < pixels.Length; i++) {
                // RTのアルファ値が1ならば上書き
                if(pixels[i].a == 1) {
                    dest[i] = pixels[i];
                }
            }
            RenderTexture.ReleaseTemporary(renderTexture);
            output.SetPixels(dest);
            output.Apply();
            return baseTex;
        }

        /// <summary>
        /// RenderTextureからピクセル情報を取得する
        /// </summary>
        private static Color[] GetPixelsFromRT(RenderTexture target) {
            var preRT = RenderTexture.active;
            RenderTexture.active = target;

            // ReadPixels()でレンダーターゲットからテクスチャ情報を生成する
            var texture = new Texture2D(target.width, target.height);
            texture.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
            texture.Apply();

            RenderTexture.active = preRT;
            return texture.GetPixels();
        }

        public static Texture2D ToTexture2D(Texture tex) {
            var sw = tex.width;
            var sh = tex.height;
            var format = TextureFormat.RGBA32;
            var result = new Texture2D( sw, sh, format, false );
            var currentRT = RenderTexture.active;
            var rt = new RenderTexture( sw, sh, 32 );
            Graphics.Blit( tex, rt );
            RenderTexture.active = rt;
            var source = new Rect( 0, 0, rt.width, rt.height );
            result.ReadPixels( source, 0, 0 );
            result.Apply();
            RenderTexture.active = currentRT;
            return result;
        }
    }
}
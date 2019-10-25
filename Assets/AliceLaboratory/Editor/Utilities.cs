using UnityEngine;

namespace AliceLaboratory.Editor {
    public class Utilities {

        /// <summary>
        /// テクスチャの重ね合わせ処理
        /// </summary>
        public static Texture2D Overlap(Texture2D overTex, Texture2D baseTex) {
            // サイズ調整用のRenderTextureを用意
            var renderTexture = RenderTexture.GetTemporary(baseTex.width, baseTex.height);
            
            Graphics.Blit(baseTex, renderTexture);
            var basePixels = GetPixelsFromRT(renderTexture);
            
            Graphics.Blit(overTex, renderTexture);
            var overPixels = GetPixelsFromRT(renderTexture);

            // outputピクセルを作って最終的な色を書き込んでいく
            var outputPixels = new Color[basePixels.Length];
            
            for (int i = 0; i < basePixels.Length; ++i) {
                var d = basePixels[i];
                var f = overPixels[i];
                var a = f.a + d.a * (1f - f.a);
                if(a > 0f) {
                    var r = (f.r * f.a + d.r * d.a * (1f - f.a)) / a;
                    var g = (f.g * f.a + d.g * d.a * (1f - f.a)) / a;
                    var b = (f.b * f.a + d.b * d.a * (1f - f.a)) / a;
                    outputPixels[i] = new Color(r, g, b, a);
                } else {
                    outputPixels[i] = new Color(0f, 0f, 0f, a);
                }
            }
            RenderTexture.ReleaseTemporary(renderTexture);
            var output = new Texture2D(baseTex.width, baseTex.height, TextureFormat.ARGB32, false);
            output.SetPixels(outputPixels);
            output.Apply();
            return output;
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
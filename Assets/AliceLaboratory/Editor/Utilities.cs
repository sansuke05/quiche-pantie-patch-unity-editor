using UnityEngine;

namespace AliceLaboratory.Editor {
    public class Utilities {
        public static Texture2D Overlap(Texture2D overTex, Texture2D baseTex) {
            //TODO: テクスチャの合成処理をここに追加
            return overTex;
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
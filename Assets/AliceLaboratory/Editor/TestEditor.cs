using System.IO;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class TestEditor {
        //[MenuItem( "Tools/Example" )]
        private static void Example() {
            var type = typeof( GameObject );
            var content = EditorGUIUtility.ObjectContent( null, type );
            var image = content.image;
            var tex = Convert(image);
            var png = tex.EncodeToPNG();
            File.WriteAllBytes( "result.png", png );
        }

        public static Texture2D Convert(Texture texture) {
            return Utilities.ToTexture2D(texture);
        }
    }
}
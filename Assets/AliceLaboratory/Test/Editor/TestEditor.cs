using System.IO;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class TestEditor {
        private const string BASE_PATH = "Test/";

        [MenuItem( "Test/Execution" )]
        private static void Example() {
            var type = typeof( GameObject );
            var content = EditorGUIUtility.ObjectContent( null, type );
            var image = content.image;
            var tex = Convert(image);
            var f = new FilerOperator();
            f.Create("ressult.png", BASE_PATH + "Results", tex);
            Debug.Log("Test is Done");
        }

        public static Texture2D Convert(Texture texture) {
            return TextureUtils.ToTexture2D(texture);
        }
    }
}
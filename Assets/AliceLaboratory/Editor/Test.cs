using System.Collections;
using UnityEditor;
using UnityEngine;

namespace AliceLaboratory.Editor {
    public class Test {
        public IEnumerator DelayLog() {
            Debug.Log("Coroutine Test Start...");

            var timeSinceStartup = EditorApplication.timeSinceStartup;

            while (EditorApplication.timeSinceStartup - timeSinceStartup < 0.5f) {
                yield return 0;
            }
            
            Debug.Log("Coroutine Test End!");
        }
    }
}
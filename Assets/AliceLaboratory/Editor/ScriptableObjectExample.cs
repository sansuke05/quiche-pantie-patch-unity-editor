using System;
using UnityEngine;

[Serializable]
public class ScriptableObjectExample : ScriptableObject {

    [SerializeField]
    private Material _exampleMat;

    public Material ExampleMat {
        get { return _exampleMat; }
#if UNITY_EDITOR
        set { _exampleMat = value; }
#endif
    }
}
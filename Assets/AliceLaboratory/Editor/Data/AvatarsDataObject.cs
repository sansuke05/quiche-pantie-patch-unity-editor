using System;
using UnityEngine;

[Serializable]
public class AvatarsDataObject : ScriptableObject {
    [SerializeField]
    private string[] _displayNames;

    [SerializeField]
    private string[] _models;

    public string[] DisplayNames {
        get { return _displayNames; }
#if UNITY_EDITOR
        set { _displayNames = value; }
#endif
    }

    public string[] Models {
        get { return _models; }
#if UNITY_EDITOR
        set { _models = value; }
#endif
    }
}
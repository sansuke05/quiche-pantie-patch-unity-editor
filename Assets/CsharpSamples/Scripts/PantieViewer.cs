using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PantieViewer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string dreamDir = "Assets/AliceLaboratory/Dreams/";
        string path = dreamDir + "0001.png";

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(File.ReadAllBytes(path));

        Renderer r = GetComponent<Renderer>();
        r.material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

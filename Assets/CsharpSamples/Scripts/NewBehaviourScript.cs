using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 scale = this.transform.localScale;
        scale.x = -scale.x;

        this.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

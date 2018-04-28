using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlash : MonoBehaviour {

    public float dathTime = 1.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, dathTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

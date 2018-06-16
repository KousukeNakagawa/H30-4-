using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Camera.main != null)
        {
            Vector3 target = Camera.main.transform.position;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
		
	}
}

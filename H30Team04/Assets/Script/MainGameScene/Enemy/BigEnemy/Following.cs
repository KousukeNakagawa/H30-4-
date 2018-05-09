using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    public Transform followTrans;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = followTrans.position;
	}
}

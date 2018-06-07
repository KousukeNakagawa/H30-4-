using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenshinn : MonoBehaviour {

    public float speed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
	}
}

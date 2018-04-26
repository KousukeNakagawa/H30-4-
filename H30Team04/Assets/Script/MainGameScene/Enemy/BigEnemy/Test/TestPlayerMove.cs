using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour {

    public float speed = 8.0f;
    public float turnSpeed = 40.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float rotate = Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Vertical");

        transform.Translate(move * speed * Time.deltaTime, 0, 0);
        transform.Rotate(0, rotate * turnSpeed * Time.deltaTime, 0, Space.World);
	}
}

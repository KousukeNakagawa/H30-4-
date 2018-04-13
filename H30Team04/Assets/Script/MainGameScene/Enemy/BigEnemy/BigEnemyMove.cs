using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyMove : MonoBehaviour {

    public float x_move = 8.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(x_move * Time.deltaTime, 0, 0);
	}
}

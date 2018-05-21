using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// camera goto no

public class TestCamera : MonoBehaviour {

    TestSSS manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float distanceToPlayer = 0.0f;

        manager.UpdateCameraDistance(gameObject, distanceToPlayer);
	}
}

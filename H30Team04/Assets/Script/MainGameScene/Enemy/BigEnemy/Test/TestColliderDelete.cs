using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColliderDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BigEnemy"))
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}

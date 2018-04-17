using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyCreate : MonoBehaviour {

    public GameObject smallEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            Vector3 smallPos = transform.position;
            //smallPos.y -= 20.0f;
            GameObject small = Instantiate(smallEnemy, smallPos, Quaternion.identity);
        }
	}
}

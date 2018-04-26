using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraUpdown : MonoBehaviour {

    public GameObject player;
    public float updownSpeed = 40.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetKey(KeyCode.J))
        {  //テストアップ
            transform.RotateAround(player.transform.position, player.transform.TransformDirection(Vector3.forward),
                updownSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.K))
        {  //テストダウン
            transform.RotateAround(player.transform.position, player.transform.TransformDirection(Vector3.forward),
                -updownSpeed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraRotate : MonoBehaviour {

    public float updownSpeed = 40.0f;
    public GameObject player;
    private TestPlayerMove move;

	// Use this for initialization
	void Start () {
        move = player.GetComponent<TestPlayerMove>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.RotateAround(player.transform.position, player.transform.TransformDirection(Vector3.up),
    move.turnSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
    }
}

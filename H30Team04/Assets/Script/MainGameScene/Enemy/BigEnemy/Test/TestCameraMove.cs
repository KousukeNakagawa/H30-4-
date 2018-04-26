using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMove : MonoBehaviour {

    public GameObject player;
    private TestPlayerMove move;
    public Transform child;

	// Use this for initialization
	void Start () {
        move = player.GetComponent<TestPlayerMove>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 dir = child.right;
        transform.Translate(dir * Input.GetAxis("Vertical") * move.speed * Time.deltaTime, Space.Self);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    //public GameObject target;

    [SerializeField] private Transform rotatePoint;
    [SerializeField] private float rotateSpeed = 60.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.RotateAround(rotatePoint.position, Vector3.up, -rotateSpeed * Time.deltaTime);
    }
}

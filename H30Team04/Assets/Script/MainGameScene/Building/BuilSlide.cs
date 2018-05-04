using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilSlide : MonoBehaviour {

    private float startPosX = 0;
    private float endPosX = 0;
    private float speed = 5;

	// Use this for initialization
	void Start () {
        SlideParent sp = transform.parent.GetComponent<SlideParent>();
        startPosX = sp.StartPosX;
        endPosX = sp.EndPosX;
        speed = sp.Speed;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position -= Vector3.right * speed * Time.deltaTime;
        if(transform.position.x < endPosX)
        {
            transform.position = new Vector3(startPosX, 0, transform.position.z);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffectScaleDown : MonoBehaviour {

    public float downSpeed = 40.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x <= 0 && transform.localScale.z <= 0) Destroy(gameObject);
        transform.localScale -= new Vector3(downSpeed, downSpeed, downSpeed) * Time.deltaTime;
 	}
}

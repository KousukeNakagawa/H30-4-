using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoop : MonoBehaviour {

    public Transform startPos;
    public Transform endPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(endPos.position.x < transform.position.x)
        {
            Vector3 returnPos = transform.position;
            returnPos.x = startPos.position.x;
            transform.position = returnPos;
        }
	}
}

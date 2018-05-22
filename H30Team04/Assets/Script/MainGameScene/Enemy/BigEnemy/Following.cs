using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    [Tooltip("追従するオブジェクト")]public Transform followTrans;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //座標を固定する
        transform.position = followTrans.position;
	}
}

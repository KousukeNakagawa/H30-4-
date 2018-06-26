using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    [Tooltip("追従するオブジェクト")]public Transform followTrans;
	
	// Update is called once per frame
	void Update () {
        //座標を固定する
        transform.position = followTrans.position;
	}
}

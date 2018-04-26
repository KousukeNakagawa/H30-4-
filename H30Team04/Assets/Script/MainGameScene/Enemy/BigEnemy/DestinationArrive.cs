using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationArrive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos = BigEnemyScripts.searchObject.targetPos;
        if (targetPos != Vector3.zero)
        {
            //目的地に到着していたら突進を終了する
            if ((targetPos - BigEnemyScripts.mTransform.position).magnitude < 10.0f)
            {
                BigEnemyScripts.searchObject.ResetTarget();
                BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
            }
        }
	}
}

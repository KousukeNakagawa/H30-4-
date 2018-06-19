//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class DestinationArrive : MonoBehaviour {

    [Tooltip("目的地との距離"),SerializeField] private float targetMagnitudeRange = 5.0f;

	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) return;
        Vector3 targetPos = BigEnemyScripts.searchObject.targetPos;
        if (targetPos != Vector3.zero)
        {
            //目的地に到着していたら突進を終了する
            if ((targetPos.ToTopView() - BigEnemyScripts.mTransform.position.ToTopView()).magnitude < targetMagnitudeRange)
            {
                BigEnemyScripts.searchObject.ResetTarget();
                BigEnemyScripts.bigEnemyMove.SetGoDefenseLine();
                BigEnemyScripts.bigEnemyAnimatorManager.isDash = false;
            }
        }
	}
}

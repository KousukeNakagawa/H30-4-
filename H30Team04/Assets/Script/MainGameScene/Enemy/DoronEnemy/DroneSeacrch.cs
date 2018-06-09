using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSeacrch : MonoBehaviour
{
    [SerializeField,Tooltip("自分のDroneMove2")] private DroneMove2 droneMove;

    void OnTriggerStay(Collider other)
    {  //探索範囲がプレイヤーと当たったら追従を開始する
        if (other.CompareTag("Player") && !BigEnemyScripts.missileLaunch.isMissile && !BigEnemyScripts.bigEnemyMove.isTurn &&
            droneMove.followObj == null)
        {
            droneMove.followObj = other.transform;
            BigEnemyScripts.missileLaunch.isMissile = true;
            BigEnemyScripts.searchObject.targetPos = other.transform.position;
            BigEnemyScripts.missileLaunch.LaunchSet();
            GetComponentInParent<DroneAudioPlay>().Play(DroneAudioPlay.AudioType.Search, true);
        }
    }
}

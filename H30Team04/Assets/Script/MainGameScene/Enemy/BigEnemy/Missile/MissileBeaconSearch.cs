using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBeaconSearch : MonoBehaviour
{
    [Tooltip("ビーコンの検索範囲")] public float searchRange = 30.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (BigEnemyScripts.missileLaunch.isMissile)
        {
            if (!BigEnemyScripts.missileLaunch.isLaunch) return;
            //予備動作中なら以下を実行する
            SortedList<float, GameObject> beacons = new SortedList<float, GameObject>();
            foreach (GameObject b in GameObject.FindGameObjectsWithTag("Beacon"))
            {
                if (!beacons.ContainsKey((b.transform.position - BigEnemyScripts.mTransform.position).sqrMagnitude))
                {
                    beacons.Add((b.transform.position - BigEnemyScripts.mTransform.position).sqrMagnitude, b);
                }
            }
            foreach (var beacon in beacons)
            {
                if (beacon.Key <= searchRange)
                {
                    BigEnemyScripts.searchObject.MissileTargetChange(beacon.Value);
                }
                //一番距離が近いビーコンのみを比較する
                break;
            }
        }
    }
}

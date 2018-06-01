using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimRange : MonoBehaviour
{
    //「ドローン」「ミサイル」のロックオン用
    SortedList<GameObject, float>
        sortTargetDistance = new SortedList<GameObject, float>();

    GameObject player;
    PlayerBase playerBase;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();
    }

    void OnTriggerStay(Collider collider)
    {
        //if (!playerBase.GetIsEndSE()) return;
        //if (player == null) return;

        ////ドローンの取得
        //if (collider.CompareTag("SmallEnemy"))
        //{
        //    var drawns = GameObject.FindGameObjectsWithTag("SmallEnemy");

        //    foreach (var drawn in drawns)
        //    {
        //        //プレイヤーに対しての方向を取得
        //        Vector3 drawnPos = drawn.transform.position;
        //        Vector3 direction = drawnPos - player.transform.position;

        //        //重複していなければ距離をソートし登録
        //        if (!sortTargetDistance.ContainsKey(drawn))
        //            sortTargetDistance.Add(drawn, direction.sqrMagnitude);
        //    }
        //}

        ////ミサイルの取得
        //if (collider.CompareTag("Missile"))
        //{
        //    var missiles = GameObject.FindGameObjectsWithTag("Missile");

        //    foreach (var missile in missiles)
        //    {
        //        //プレイヤーに対しての方向を取得
        //        Vector3 missilePos = missile.transform.position;
        //        Vector3 direction = missilePos - player.transform.position;

        //        //重複していなければ距離をソートし登録
        //        if (!sortTargetDistance.ContainsKey(missile))
        //            sortTargetDistance.Add(missile, direction.sqrMagnitude);
        //    }
        //}
    }

    void OnTriggerExit(Collider collider)
    {
        //ロックオン不可能になる
        if (sortTargetDistance.ContainsKey(collider.gameObject))
            sortTargetDistance.RemoveAt(sortTargetDistance.IndexOfKey(collider.gameObject));
    }

    /// <summary>
    /// ＊「ドローン」と「ミサイル」ロックオンリスト
    /// </summary>
    public SortedList<GameObject, float> GetTarget()
    {
        return sortTargetDistance;
    }
}

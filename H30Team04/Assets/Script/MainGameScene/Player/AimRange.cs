using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AimRange : MonoBehaviour
{
    //「ドローン」「ミサイル」のロックオン用
    SortedList<float, GameObject>
        sortTargetDistance = new SortedList<float, GameObject>();

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerStay(Collider collider)
    {
        if (player == null) return;

        //ドローンの取得
        if (collider.CompareTag("SmallEnemy"))
        {
            var drawns = GameObject.FindGameObjectsWithTag("SmallEnemy");

            foreach (var drawn in drawns)
            {
                //プレイヤーに対しての方向を取得
                Vector3 drawnPos = drawn.transform.position;
                Vector3 direction = drawnPos - player.transform.position;

                //重複していなければ距離をソートし登録
                if (!sortTargetDistance.ContainsValue(drawn))
                    sortTargetDistance.Add(direction.sqrMagnitude, drawn);
            }
        }

        //ミサイルの取得
        if (collider.CompareTag("Missile"))
        {
            var missiles = GameObject.FindGameObjectsWithTag("Missile");

            foreach (var missile in missiles)
            {
                //プレイヤーに対しての方向を取得
                Vector3 missilePos = missile.transform.position;
                Vector3 direction = missilePos - player.transform.position;

                //重複していなければ距離をソートし登録
                if (!sortTargetDistance.ContainsValue(missile))
                    sortTargetDistance.Add(direction.sqrMagnitude, missile);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //ロックオン不可能になる
        if (sortTargetDistance.ContainsValue(collider.gameObject))
            sortTargetDistance.RemoveAt(sortTargetDistance.IndexOfValue(collider.gameObject));
    }

    /// <summary>
    /// ＊「ドローン」と「ミサイル」ロックオンリスト
    /// </summary>
    public SortedList<float, GameObject> GetTarget()
    {
        return sortTargetDistance;
    }
}

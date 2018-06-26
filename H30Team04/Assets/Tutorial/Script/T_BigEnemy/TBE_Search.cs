using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> TBE 進行方向クラス </summary>
public class TBE_Search : MonoBehaviour
{
    /// <summary> 進行方向 (対象) の候補 </summary>
    enum Targets
    {
        Nothing, Beacon, Player
    }

    /// <summary> 現在の対象 </summary>
    Targets currentT = Targets.Nothing;

    /// <summary> 対象ごとにその位置を格納 </summary>
    Dictionary<Targets, Vector3>
        target = new Dictionary<Targets, Vector3>();

    /// <summary> 対象のトランスフォーム </summary>
    Transform nothingT, beaconT, playerT;
    /// <summary> 対象の位置 </summary>
    Vector3 nothing, beacon, player;

    /// <summary> 進行方向 (対象) </summary>
    public static Vector3 Target { get; private set; }

    void Start()
    {
        // プレイヤーの初期位置を無対象時の対象にする
        nothing = GameObject.FindGameObjectWithTag("Player").transform.position;
        // 各ペアの登録
        target.Add(Targets.Nothing, nothing);
        target.Add(Targets.Beacon, beacon);
        target.Add(Targets.Player, player);
    }

    void Update()
    {
        // 現在の対象の位置
        Target = target[currentT];
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        beacon = GameObject.FindGameObjectWithTag("Beacon").transform.position;
        // 対象の位置に到達した際の処理
        GoalTarget();
    }

    void OnTriggerStay(Collider other)
    {
        // ビーコンを発見した場合
        if (other.gameObject.CompareTag("Beacon"))
        {

            currentT = Targets.Beacon;
        }
        // プレイヤーを発見した場合
        else if (other.gameObject.CompareTag("Player"))
        {

            currentT = Targets.Player;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ビーコンかプレイヤーを見失った場合
        if (other.gameObject.CompareTag("Beacon") ||
            other.gameObject.CompareTag("Player")) currentT = Targets.Nothing;
    }

    /// <summary> 対象の位置に到達したら </summary>
    void GoalTarget()
    {
        // 対象と自身の距離
        var distance = Vector3.Distance(transform.position, target[currentT]);

        // 距離が10より近くなったら動きを止める
        TBE_Move.IsAdvance = !(Mathf.Abs(distance) < 10);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 範囲にプレイヤーが到達すると知らせる移動チュートリアル用オブジェクト </summary>
public class GoalPoint : MonoBehaviour
{
    [SerializeField, Range(0, 89), Tooltip("到達確認範囲")] float range;
    [SerializeField, Tooltip("onにすると範囲を確認可能")] bool isCheck = true;
    [SerializeField, Range(1, 360), Tooltip("描画する線数")] int lines = 1;
    GameObject player;

    /// <summary> プレイヤーが到達したかどうか </summary>
    public static bool IsGoal { get; private set; }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        IsGoal = false;
    }

    void Update()
    {
        IsGoal = IsArrival();
    }

    void OnDrawGizmos()
    {
        if (isCheck) DrawArea();
    }

    /// <summary> プレイヤーの到達判定 </summary>
    bool IsArrival()
    {
        //プレイヤーとの距離
        var dir = player.transform.position - transform.position;
        //差分角度
        var angle = Vector3.Angle(transform.forward, dir);
        //範囲にプレイヤーが入ったら true
        return Mathf.Abs(angle) <= range;
    }

    /// <summary> 範囲確認用 </summary>
    void DrawArea()
    {
        //色の指定
        Gizmos.color = Color.red;
        //表示させる線の数
        var num = 360 / lines;
        //長さ
        var lenght = transform.position.y / Mathf.Sin(Mathf.Deg2Rad * (180 - 90 - range));
        //方向と長さ
        var forward = transform.forward * lenght;

        //視界の右端の描画
        for (int i = 0; i < 360 / num; i++)
            Gizmos.DrawRay(transform.position, Quaternion.Euler(range, i * num, 0) * forward);
    }
}

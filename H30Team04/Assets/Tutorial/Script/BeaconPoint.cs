using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ビーコン発射チュートリアル用オブジェクト </summary>
public class BeaconPoint : MonoBehaviour
{
    [SerializeField, Range(0, 89), Tooltip("到達確認範囲")] float range;
    //[SerializeField, Tooltip("onにすると範囲を確認可能")] bool isCheck = true;
    //[SerializeField, Range(1, 360), Tooltip("描画する線数")] int lines = 1;

    /// <summary> ビーコン目標地点に止まったか </summary>
    public static bool IsBeaconHit { get; private set; }
    public static bool IsBigEnemyHit { get; private set; }

    void Start()
    {
        IsBeaconHit = false;
        IsBigEnemyHit = false;
    }

    void Update()
    {
        IsBeaconHit = IsArrival();
    }

    void OnTriggerEnter(Collider other)
    {
        //ビーコンにビッグエネミーが誘導されたら
        if (IsBeaconHit && other.gameObject.CompareTag("BigEnemy")) IsBigEnemyHit = true;
    }

    void OnDrawGizmos()
    {
        //if (isCheck) DrawArea();
    }

    /// <summary> プレイヤーの到達判定 </summary>
    bool IsArrival()
    {
        if (GameObject.FindGameObjectWithTag("Beacon") == null) return false;
        //プレイヤーとの距離
        var dir = GameObject.FindGameObjectWithTag("Beacon").transform.position - transform.position;
        //差分角度
        var angle = Vector3.Angle(transform.forward, dir);
        //範囲にプレイヤーが入ったら true
        return Mathf.Abs(angle) <= range;
    }

    /// <summary> 範囲確認用 </summary>
    void DrawArea()
    {
        ////色の指定
        //Gizmos.color = Color.blue;
        ////表示させる線の数
        //var num = 360 / lines;
        ////長さ
        //var lenght = 2.5f / Mathf.Sin(Mathf.Deg2Rad * (180 - 90 - range));
        ////方向と長さ
        //var forward = transform.forward * lenght;

        ////視界の右端の描画
        //for (int i = 0; i < 360 / num; i++)
        //    Gizmos.DrawRay(transform.position, Quaternion.Euler(range, i * num, 0) * forward);
    }
}
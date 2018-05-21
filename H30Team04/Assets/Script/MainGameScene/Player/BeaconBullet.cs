using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconBullet : MonoBehaviour
{
    [SerializeField] GameObject beacon;
    [SerializeField] [Range(1, 300)] float speed = 50; //弾速
    [SerializeField] [Range(5, 100)] float rangeDistance = 50; //射程距離
    [SerializeField, Range(10, 300)] float extinctionTime = 10; //消滅時間（秒）

    Rigidbody rb;
    Vector3 startPos; //初期位置
    bool isChange; //タグ変化把握用

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
        isChange = false;
    }

    void Update()
    {
        OverRange(); //射程外消滅処理
        Extinction(); //時間消滅処理
    }

    void OnCollisionEnter(Collision other)
    {
        //プレイヤー・スナイパーらとの衝突は無視
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Sniper")) return;

        //エネミーとミサイルに衝突時、自身を破壊する
        else if (other.collider.CompareTag("BigEnemy") || other.collider.CompareTag("SmallEnemy") || other.collider.CompareTag("Missile")) Destroy(beacon);

        //ビル・地面に衝突時、くっつく
        else if (other.collider.CompareTag("Building") || other.collider.CompareTag("Field")) Cling(other);
    }

    /// <summary>
    /// 射程外消滅処理
    /// </summary>
    void OverRange()
    {
        //飛距離
        Vector3 FlyDistance = rb.position - startPos;

        //飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > rangeDistance) Destroy(beacon);
    }

    /// <summary>
    /// ビーコンの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        rb.velocity = direction * speed;
    }

    /// <summary>
    /// 張り付き処理
    /// </summary>
    void Cling(Collision other)
    {
        //スピードを止める
        rb.velocity = Vector3.zero;

        //tagを「BeaconBullet」から「Beacon」へ
        transform.tag = "Beacon";
        isChange = true;

        //当たったオブジェクトの子になる
        transform.parent = other.transform;
    }

    /// <summary>
    /// 時間消滅処理
    /// </summary>
    void Extinction()
    {
        if (!isChange) return;

        //消滅時間をカウント
        extinctionTime -= Time.deltaTime;

        //消滅時間になったら消滅
        if (extinctionTime <= 0) Destroy(beacon);
    }

    /// <summary>
    /// 射程距離のゲッター
    /// </summary>
    public float GetRangeDistance()
    {
        return rangeDistance;
    }

    /// <summary>
    /// ビーコン・ビーコンバレットが一つもなければ発射許可
    /// </summary>
    //public bool IsFireOK()
    //{
    //    return (!GameObject.FindGameObjectWithTag("Beacon") &&
    //        !GameObject.FindGameObjectWithTag("BeaconBullet")) ? true : false;
    //}
}

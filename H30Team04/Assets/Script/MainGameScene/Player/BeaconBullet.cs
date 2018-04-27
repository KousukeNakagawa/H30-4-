using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconBullet : MonoBehaviour
{
    [SerializeField] GameObject beacon;
    [SerializeField] [Range(1, 100)] float speed = 50; //弾速
    [SerializeField] [Range(5, 100)] float rangeDistance = 50; //射程距離
    [SerializeField, Range(10, 300)] float extinctionTime = 10; //消滅時間（秒）

    Rigidbody rb;
    Vector3 startPos;
    bool isChange; //タグ変化把握用

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
        isChange = false;
    }

    void Update()
    {
        OverRangeDistance(); //射程外消滅
        Extinction(); //時間消滅
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) return;

        //ドローンかミサイルにヒットしたら、それを消滅させる
        if (collider.CompareTag("Building") || collider.CompareTag("Field")) Cling();
        //else Destroy(beacon);
    }

    /// <summary>
    /// ＊射程外消滅
    /// </summary>
    void OverRangeDistance()
    {
        //飛距離
        Vector3 FlyDistance = rb.position - startPos;

        //飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > rangeDistance) Destroy(beacon);
    }

    /// <summary>
    /// ＊ビーコンの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        rb.velocity = direction * speed;
    }

    /// <summary>
    /// ＊張り付き
    /// </summary>
    void Cling()
    {
        //今後一切の動きを停止
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //tagを「BeaconBullet」から「Beacon」へ
        transform.tag = "Beacon";
        isChange = true;
    }

    /// <summary>
    /// ＊時間消滅
    /// </summary>
    void Extinction()
    {
        if (!isChange) return;

        //消滅時間を数える
        extinctionTime -= Time.deltaTime;

        //消滅時間になったら消滅
        if (extinctionTime <= 0) Destroy(beacon);
    }

    /// <summary>
    /// ＊射程距離のゲッター
    /// </summary>
    public float GetRangeDistance()
    {
        return rangeDistance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [SerializeField] GameObject snipeBullet;
    [SerializeField] [Range(1, 300)] float speed = 100; //弾速
    [SerializeField] [Range(5, 300)] float rangeDistance = 100; //射程距離

    Rigidbody rb;
    Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
    }

    void Update()
    {
        OverRangeDistance(); //射程距離外消滅
    }

    void OnTriggerEnter(Collider collider)
    {
        //ドローンかミサイルにヒットしたら、それを消滅させる
        if (collider.CompareTag("SmallEnemy") || collider.CompareTag("Missile"))
        {
            GameObject Dead = collider.gameObject;
            Destroy(Dead);
            Dead = null;
        }
    }

    /// <summary>
    /// ＊射程外消滅
    /// </summary>
    void OverRangeDistance()
    {
        //飛距離
        Vector3 FlyDistance = rb.position - startPos;

        //飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > rangeDistance) Destroy(snipeBullet);
    }

    /// <summary>
    /// ＊スナイパーバレットの発射
    /// </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        rb.velocity = direction * speed;
    }

    /// <summary>
    /// ＊射程距離のゲッター
    /// </summary>
    public float GetRangeDistance()
    {
        return rangeDistance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [SerializeField] GameObject snipeBullet;
    [SerializeField] [Range(0, 300)] float speed = 100; //弾速
    [SerializeField] [Range(5, 300)] static float rangeDistance = 100; //射程距離

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
        //地面・ビルと衝突時、自身を消滅
        if (collider.CompareTag("Field") || collider.CompareTag("Building")) Destroy(snipeBullet);
    }

    private void OnCollisionEnter(Collision other)
    {
        //地面・ビルと衝突時、自身を消滅
        if (other.collider.CompareTag("Field") || other.collider.CompareTag("Building")) Destroy(snipeBullet);
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
    public static float GetRangeDistance()
    {
        return rangeDistance;
    }

    /// <summary>
    /// SnipeBulletを検索し、他にいなければ許可
    /// </summary>
    //public bool IsFireOK()
    //{
    //    return (!GameObject.FindGameObjectWithTag("SnipeBullet")) ? true : false;
    //}
}

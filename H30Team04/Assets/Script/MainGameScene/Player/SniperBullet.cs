using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    /// <summary> 弾速 </summary>
    [SerializeField] [Range(0, 300)] float speed_ = 100;

    /// <summary> リジッドボディ </summary>
    Rigidbody rb_;
    /// <summary> 発生位置 </summary>
    Vector3 origin_;

    /// <summary> 射程距離 </summary>
    public static float RangeDistance_ { get { return 100; } }

    void Start()
    {
        // コンポーネント
        rb_ = GetComponent<Rigidbody>();
        origin_ = rb_.position;
    }

    void Update()
    {
        // 射程外消滅
        OverRangeDistance();
    }

    void OnTriggerEnter(Collider collider)
    {
        // 地面・ビルと衝突時、自身を消滅
        if (collider.CompareTag("Building") ||
            collider.CompareTag("Field")) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        // 地面・ビルと衝突時、自身を消滅
        if (other.collider.CompareTag("Building") ||
            other.collider.CompareTag("Field") ||
            other.collider.name.Contains("Hide")) Destroy(gameObject);
    }

    /// <summary> 射程外消滅 </summary>
    void OverRangeDistance()
    {
        // 飛距離
        Vector3 FlyDistance = rb_.position - origin_;

        // 飛距離が射程距離を超えたら消滅
        if (FlyDistance.magnitude > RangeDistance_) Destroy(gameObject);
    }

    /// <summary> スナイパー弾の発射 </summary>
    public void Fire(Vector3 direction)
    {
        Start();
        var speed = (Time.timeScale == 0) ? 0 : this.speed_;
        rb_.velocity = direction * speed;
    }

    /// <summary> 射程距離 </summary>
    public static float GetRangeDistance()
    {
        return RangeDistance_;
    }

    /// <summary>
    /// SnipeBulletを検索し、他にいなければ許可
    /// </summary>
    //public bool IsFireOK()
    //{
    //    return (!GameObject.FindGameObjectWithTag("SnipeBullet")) ? true : false;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> マシンガンの弾クラス </summary>
public class MG_Bullet : MonoBehaviour
{
    /// <summary> 弾速 </summary>
    [SerializeField] [Range(0, 300)] float speed_ = 100;
    /// <summary> 射程 </summary>
    [SerializeField] [Range(5, 300)] float range_ = 100;
    /// <summary> 初期位置 </summary>
    Vector3 bornPos_;
    /// <summary> リジッドボディ </summary>
    Rigidbody rb_;

    /// <summary> 自身が発射されたか </summary>
    public bool IsFire { get; set; }

    void Update()
    {
        OverRange();
    }

    /// <summary> 準備処理 </summary>
    public void Init()
    {
        rb_ = GetComponent<Rigidbody>();
        IsFire = false;
    }

    /// <summary> 射程外処理 </summary>
    void OverRange()
    {
        // 飛距離
        Vector3 FlyDistance = rb_.position - bornPos_;
        // 射程を超えたら
        if (FlyDistance.magnitude > range_)
        {

        }
    }

    public void Fire(Vector3 direction)
    {
        // 一時停止中は止める
        var speed = (Time.timeScale == 0) ? 0 : speed_;
        // 指定した方向へ飛ぶ
        rb_.velocity = direction * speed;
    }
}

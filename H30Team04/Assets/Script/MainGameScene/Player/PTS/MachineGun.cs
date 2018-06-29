using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> マシンガンクラス </summary>
public class MachineGun : MonoBehaviour
{
    /// <summary> 弾数 </summary>
    [SerializeField, Range(30, 100)] int bulletsNum_ = 100;
    /// <summary> 連射性能 </summary>
    [SerializeField, Range(0, 1)] float burstAbility_ = 0.1f;

    /// <summary> 連射性能 </summary>
    [SerializeField] Transform muzzle_;

    /// <summary> 最大弾数 </summary>
    int maxBN_;
    /// <summary> 連射性能のバックアップ </summary>
    float backupBurstAb_;

    void Start()
    {
        maxBN_ = bulletsNum_;
        backupBurstAb_ = burstAbility_;
    }

    void Update()
    {

    }

    /// <summary> 指定した方向へ弾を発射 </summary>
    void Fire(Ray ray)
    {
        // 発射する弾
        var bullet = MGB_MG.Bullet();
        // 発射位置
        bullet.transform.position = muzzle_.position - Vector3.up * 0.2f + Vector3.forward * 0.2f;

        burstAbility_ -= Time.deltaTime;
        if (burstAbility_ <= 0)
        {
            bullet.GetComponent<MG_Bullet>().Fire(ray.direction);
        }
    }
}

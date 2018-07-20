using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 標準エフェクトクラス </summary>
public class AimEffect : MonoBehaviour
{
    /// <summary> 標準を管理 </summary>
    [SerializeField] GameObject aimB;
    [SerializeField] GameObject aimR;

    /// <summary> スケール変動値 </summary>
    [SerializeField, Range(0.1f, 2)] float power = 1;
    /// <summary> スケール変動地 </summary>
    [SerializeField, Range(0.1f, 2)] float modify = 2;
    /// <summary> 最小サイズ </summary>
    [SerializeField, Range(1, 3)] float minMotionScale = 3;
    /// <summary> 縮小サイズ </summary>
    [SerializeField, Range(0.1f, 1)] float reductionScale = 0.3f;
    /// <summary> サイズ割合 </summary>
    [SerializeField, Range(1, 5)] float scaleLate = 5;
    /// <summary> サイズ縮小速度 </summary>
    [SerializeField, Range(0.1f, 1)] float scaleLerpSpeed = 0.2f;

    /// <summary> 静的 gameObject </summary>
    public static GameObject gameObject_;
    /// <summary> 静的 transform </summary>
    public static Transform transform_;

    /// <summary> 静的 スケール変動地 </summary>
    public static float power_;
    /// <summary> 静的 スケール変動地 </summary>
    public static float modify_;
    /// <summary> 静的 最小サイズ </summary>
    public static float minMotionScale_;
    /// <summary> 静的 最小サイズ </summary>
    public static float reductionScale_;
    /// <summary> 静的 サイズ割合 </summary>
    public static float scaleLate_;
    /// <summary> 静的 サイズ割合 </summary>
    public static float scaleLerpSpeed_;

    void Awake()
    {
        // 疑似静的化
        gameObject_ = gameObject;
        transform_ = transform;
        power_ = power;
        modify_ = power_ + modify;
        minMotionScale_ = minMotionScale;
        reductionScale_ = reductionScale;
        scaleLate_ = scaleLate;
        scaleLerpSpeed_ = scaleLerpSpeed;
    }

    void Update()
    {
        // 武器ごとの標準の表示 ON / OFF
        aimB.SetActive(WeaponCtrl.IsWeaponBeacon);
        aimR.SetActive(!WeaponCtrl.IsWeaponBeacon);
    }

    /// <summary> 標準のモーション </summary>
    public static void AimMotion(Vector3 angle, Vector3 pos)
    {
        // スケール値
        var scale = Mathf.Cos(Time.time / power_) / modify_ + minMotionScale_;

        // スケール（プレイヤーの動きが止まったら縮小する）
        transform_.localScale = (!Soldier.IsMove && !Soldier.IsRotate) ?
             Vector3.Lerp(transform_.localScale, Vector3.one * reductionScale_, scaleLerpSpeed_) :
        Vector3.Lerp(transform_.localScale, new Vector3(scale, scale) / scaleLate_, scaleLerpSpeed_);

        // プレイヤーが動いている間 下記のモーションを実行
        if (!Soldier.IsMove && !Soldier.IsRotate) return;

        // 位置
        transform_.position = pos + angle;
        // 基本の向き
        transform_.rotation = Quaternion.LookRotation(angle);
        // 回転
        transform_.localEulerAngles += new Vector3(0, 0, Mathf.Abs(Time.time * 90));
    }
}
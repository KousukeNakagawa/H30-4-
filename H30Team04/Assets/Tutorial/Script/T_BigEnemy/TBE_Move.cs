using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBE_Move : MonoBehaviour
{
    [SerializeField, Range(0, 30), Tooltip("基本速度")] float speed = 1;
    [SerializeField, Range(30, 100), Tooltip("速度補正値")] float powerModify = 60;
    float backupSpeed;
    Rigidbody rb;

    /// <summary> 動いている状態か </summary>
    public static bool IsAdvance { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        IsAdvance = true;
        backupSpeed = speed;
    }

    void Update()
    {
        Look();
        Advance();
    }

    /// <summary> 前進処理 </summary>
    void Advance()
    {
        if (!IsAdvance) return;

        //両足が地面についていたら動きを止める
        if (TBEFL.IsHitGround && TBEFR.IsHitGround)
        {
            speed = backupSpeed;
            return;
        }
        //速度補正計算 (基本速度を毎フレーム下げる値) 
        var power = backupSpeed / powerModify;
        //どちらかの足が浮いているとき (歩いているとき) 
        if (TBEFL.IsHitGround != TBEFR.IsHitGround) speed -= power;
        if (speed <= 0) speed = 0;

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    /// <summary> 進行方向変更処理 </summary>
    void Look()
    {
        transform.LookAt(TBE_Search.Target);
    }
}

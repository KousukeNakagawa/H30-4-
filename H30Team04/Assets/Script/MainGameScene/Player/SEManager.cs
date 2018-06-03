using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField, Header("開始演出の目的地")]
    Transform endSEPoint;

    [SerializeField, Header("開始演出のON/OFF")] bool _isSE;
    [SerializeField, Header("X軸方向に進むか")] bool dirX;

    bool _isMove = true;

    /// <summary> 開始演出が終了したか </summary>
    public static bool IsEndSE { get; private set; }

    void Start()
    {
        IsEndSE = (_isSE) ? false : true;
    }

    void Update()
    {
        if (dirX) SEMove_X();
        else SEMove_Z();
    }

    /// <summary> 開始演出の自動運転（X軸） </summary>
    void SEMove_X()
    {
        if (IsEndSE) return;

        //移動量
        Vector3 move = new Vector3(endSEPoint.position.x - transform.position.x, 0);

        //移動（目的地に近づくほど減速）
        if (_isMove) transform.position += new Vector3(move.normalized.x / 10 + move.x / 100, 0);

        //開始演出の終了
        else IsEndSE = true;

        //到着
        if (Mathf.Abs(move.x) < 1) _isMove = false;
    }

    /// <summary> 開始演出の自動運転（Z軸） </summary>
    void SEMove_Z()
    {
        if (IsEndSE) return;

        //移動量
        Vector3 move = new Vector3(0, 0, endSEPoint.position.z - transform.position.z);

        //移動（目的地に近づくほど減速）
        if (_isMove) transform.position += new Vector3(0, 0, move.normalized.z / 10 + move.z / 100);

        //開始演出の終了
        else IsEndSE = true;

        //到着
        if (Mathf.Abs(move.z) < 1) _isMove = false;
    }
}
